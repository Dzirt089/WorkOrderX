using MailerVKT;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using WorkOrderX.API.ReferenceData;
using WorkOrderX.Application.Behaviors;
using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Application.Handlers.DomainEventHandler;
using WorkOrderX.Application.Queries.GetEmployeeByAccount;
using WorkOrderX.Application.Queries.GetEmployeeByAccount.Responses;
using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById;
using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.Domain.Models.EventStores;
using WorkOrderX.DomainService.ProcessRequestServices.Implementation;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;
using WorkOrderX.EFCoreDb.DbContexts;
using WorkOrderX.Http.Models;
using WorkOrderX.Infrastructure.Repositories.Implementation;

namespace WorkOrderX.API
{
	public static class HostBuilderExtensions
	{

		/// <summary>
		/// Добавляет MediatR в сервисы приложения.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddInfrastructureMediatRPipeline(this IServiceCollection services)
		{
			services.AddMediatR(cfg =>
				cfg.RegisterServicesFromAssembly(typeof(ProcessRequestStatusChangedDomainEventHandler).Assembly));

			services.AddTransient(
				typeof(IPipelineBehavior<,>),
				typeof(DomainEventsDispatchingBehavior<,>));

			services.AddTransient(
				typeof(IPipelineBehavior<,>),
				typeof(UnitOfWorkBehavior<,>));

			return services;
		}

		/// <summary>
		/// Добавляет сервисы инфраструктуры, такие как отправка писем.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddInfrastructureMailServices(this IServiceCollection services)
		{
			services.AddScoped<Sender>();
			return services;
		}

		/// <summary>
		/// Доавление сервиса для загрузки и синхронизации справочных данных в БД.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddInfrastructureHostedServices(this IServiceCollection services)
		{
			services.AddHostedService<ReferenceDataInitializer>();

			return services;
		}

		/// <summary>
		/// Добавляет <see cref="WorkOrderDbContext"/> в коллекцию сервисов и настраивает его для использования базы данных SQL Server.
		/// </summary>
		/// <remarks>Этот метод получает строку подключения из конфигурационного ключа "ConnectionStrings:WorkOrderXDatabase"
		/// и настраивает <see cref="WorkOrderDbContext"/> для работы с SQL Server с уровнем совместимости 110 (SQL Server 2012).</remarks>
		/// <param name="services">Экземпляр <see cref="IServiceCollection"/>, в который будет добавлен контекст базы данных.</param>
		/// <param name="configuration">Экземпляр <see cref="IConfiguration"/>, используемый для получения строки подключения.</param>
		/// <returns>Обновлённый экземпляр <see cref="IServiceCollection"/>.</returns>
		/// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
		public static IServiceCollection AddInfrastructureDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration["ConnectionStrings:WorkOrderXDatabase"];

			services.AddDbContext<WorkOrderDbContext>(_ =>
			{
				_.UseSqlServer(connectionString, _ =>
				//Задаем в коде максимальный уровень 110 совместимости (говорим EF Core, что он работает с SQL Server 2012)
				_.UseCompatibilityLevel(110));
			});

			services.AddScoped<IWorkplaceEmployeesRepository, WorkplaceEmployeesRepository>();
			services.AddScoped<IProcessRequestRepository, ProcessRequestRepository>();
			services.AddScoped<IEventStoreEntryRepository, EventStoreEntryRepository>();

			services.AddScoped<IProcessRequestService, ProcessRequestService>();

			return services;
		}

		/// <summary>
		/// Configures authorization and authentication services for the application.
		/// </summary>
		/// <remarks>This method adds the following authorization policies: <list type="bullet">
		/// <item><description><c>AdminOnly</c>: Requires the user to have the "Admin" or "Supervisor"
		/// role.</description></item> <item><description><c>CustomerOnly</c>: Requires the user to have the "Customer"
		/// role.</description></item> <item><description><c>ExecuterOnly</c>: Requires the user to have the "Executer"
		/// role.</description></item> </list> Additionally, it configures JWT-based authentication with token validation
		/// parameters, including issuer, audience, lifetime, and signing key validation.</remarks>
		/// <param name="services">The <see cref="IServiceCollection"/> to which the authorization and authentication services will be added.</param>
		/// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
		public static IServiceCollection AddInfrastructureAuthorization(this IServiceCollection services, IConfiguration config)
		{
			// Политики авторизации по полям
			services.AddAuthorizationBuilder()
				.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Supervisor"))
				.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"))
				.AddPolicy("ExecuterOnly", policy => policy.RequireRole("Executer"));

			// Регистрация аутентификации JWT
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					opt.TokenValidationParameters = new TokenValidationParameters
					{
						// указывает, будет ли валидироваться издатель при валидации токена
						ValidateIssuer = true,
						// строка, представляющая издателя
						ValidIssuer = config["JwtSettings:Issuer"],
						// будет ли валидироваться потребитель токена
						ValidateAudience = true,
						// установка потребителя токена
						ValidAudience = config["JwtSettings:Audience"],
						// будет ли валидироваться время существования
						ValidateLifetime = true,
						// установка ключа безопасности
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"])),
						// валидация ключа безопасности
						ValidateIssuerSigningKey = true,
					};
				});

			return services;
		}

		/// <summary>
		/// Добавляет маршруты для получения информации о сотруднике по его учетной записи.
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication AddInfrastructureAuthEmployee(this WebApplication app)
		{
			var employeeGroup = app.MapGroup("Employee");
			employeeGroup.MapGet("Login/{account}",
				async (string account, IMediator mediator, IConfiguration configuration, CancellationToken token) =>
			{
				if (string.IsNullOrEmpty(account))
					return Results.BadRequest("Account cannot be null or empty");

				GetEmployeeByAccountQuery? query = new() { UserAccount = account };
				GetEmployeeByAccountQueryResponse? response = await mediator.Send(query, token);

				if (response is null)
					return Results.Unauthorized();

				EmployeeDataModel? result = new()
				{
					Account = response.EmployeeDataDto.Account,
					Role = response.EmployeeDataDto.Role,
					Name = response.EmployeeDataDto.Name,
					Department = response.EmployeeDataDto.Department,
					Email = response.EmployeeDataDto.Email,
					Phone = response.EmployeeDataDto.Phone,
					Specialized = response.EmployeeDataDto.Specialized,
					Id = response.EmployeeDataDto.Id
				};


				var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!);
				var hours = double.Parse(configuration["JwtSettings:ExpiryHours"]!);

				var handler = new JwtSecurityTokenHandler();
				var tokenDesc = new SecurityTokenDescriptor
				{
					// Внутрь токена кладём строками: ID сотрудника, Имя, Роль.
					Subject = new CaseSensitiveClaimsIdentity([
						new Claim(ClaimTypes.NameIdentifier, response.EmployeeDataDto.Id.ToString()),
						new Claim(ClaimTypes.Name, response.EmployeeDataDto.Name),
						new Claim(ClaimTypes.Role, response.EmployeeDataDto.Role)
					]),
					// Срок жизни токена
					Expires = DateTime.Now.AddHours(hours),
					// Указываем, на каком ключе безопасности и каким алгоритмом подписать.
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256)
				};

				var tokenResult = handler.CreateToken(tokenDesc);

				return Results.Ok(new
				{
					Token = handler.WriteToken(tokenResult),
					result
				});
			});

			return app;
		}

		/// <summary>
		/// Добавляет маршруты для управления процессами заявок.
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication AddInfrastructureProcessRequest(this WebApplication app)
		{
			var processRequestGroup = app.MapGroup("ProcessRequest").RequireAuthorization();

			processRequestGroup.MapPost("CreateProcessRequest",
				[Authorize]
			async ([FromBody] CreateProcessRequestModel createProcess, IMediator mediator, CancellationToken token) =>
			{
				if (createProcess is null)
					return Results.BadRequest("Command cannot be null");

				CreateProcessRequestCommand? command = new()
				{
					ApplicationNumber = createProcess.ApplicationNumber,
					ApplicationType = createProcess.ApplicationType,
					CreatedAt = createProcess.CreatedAt,
					PlannedAt = createProcess.PlannedAt,
					EquipmentType = createProcess.EquipmentType,
					EquipmentKind = createProcess.EquipmentKind,
					EquipmentModel = createProcess.EquipmentModel,
					SerialNumber = createProcess.SerialNumber,
					TypeBreakdown = createProcess.TypeBreakdown,
					DescriptionMalfunction = createProcess.DescriptionMalfunction,
					ApplicationStatus = createProcess.ApplicationStatus,
					InternalComment = createProcess.InternalComment,
					CustomerEmployeeId = createProcess.CustomerEmployeeId,
				};

				bool result = await mediator.Send(command, token);

				return result ? Results.Ok(result) : Results.BadRequest("Failed to create process request");
			});

			processRequestGroup.MapGet("GetActivProcessRequests/{Id}",
				[Authorize]
			async (IMediator mediator, Guid Id, CancellationToken token) =>
			{
				if (Id == Guid.Empty)
					return Results.BadRequest("Command cannot be null");

				var query = new GetActivProcessRequestFromEmployeeByIdQuery { Id = Id };
				GetActivProcessRequestFromEmployeeByIdQueryResponse? response = await mediator.Send(query, token);

				IEnumerable<ProcessRequestDataModel> processRequests = response.ProcessRequests
					.Select(pr => new ProcessRequestDataModel
					{
						Id = pr.Id,
						ApplicationNumber = pr.ApplicationNumber,
						ApplicationType = pr.ApplicationType,
						CreatedAt = pr.CreatedAt,
						PlannedAt = pr.PlannedAt,
						EquipmentType = pr.EquipmentType,
						EquipmentKind = pr.EquipmentKind,
						EquipmentModel = pr.EquipmentModel,
						SerialNumber = pr.SerialNumber,
						TypeBreakdown = pr.TypeBreakdown,
						DescriptionMalfunction = pr.DescriptionMalfunction,
						ApplicationStatus = pr.ApplicationStatus,
						InternalComment = pr.InternalComment,
						CompletionAt = pr.CompletionAt,

						CustomerEmployee = new EmployeeDataModel
						{
							Id = pr.CustomerEmployee.Id,
							Account = pr.CustomerEmployee.Account,
							Role = pr.CustomerEmployee.Role,
							Name = pr.CustomerEmployee.Name,
							Department = pr.CustomerEmployee.Department,
							Email = pr.CustomerEmployee.Email,
							Phone = pr.CustomerEmployee.Phone,
							Specialized = pr.CustomerEmployee.Specialized
						},

						ExecutorEmployee = new EmployeeDataModel
						{
							Id = pr.ExecutorEmployee.Id,
							Account = pr.ExecutorEmployee.Account,
							Role = pr.ExecutorEmployee.Role,
							Name = pr.ExecutorEmployee.Name,
							Department = pr.ExecutorEmployee.Department,
							Email = pr.ExecutorEmployee.Email,
							Phone = pr.ExecutorEmployee.Phone,
							Specialized = pr.ExecutorEmployee.Specialized
						}
					});

				return Results.Ok(processRequests);
			});

			processRequestGroup.MapGet("GetHistoryProcessRequests/{Id}",
				[Authorize]
			async (IMediator mediator, Guid Id, CancellationToken token) =>
				{
					if (Id == Guid.Empty)
						return Results.BadRequest("Command cannot be null");

					var query = new GetHistoryProcessRequestFromEmployeeByIdQuery { Id = Id };
					GetHistoryProcessRequestFromEmployeeByIdQueryResponse? response = await mediator.Send(query, token);

					IEnumerable<ProcessRequestDataModel> processRequests = response.ProcessRequests
						.Select(pr => new ProcessRequestDataModel
						{
							Id = pr.Id,
							ApplicationNumber = pr.ApplicationNumber,
							ApplicationType = pr.ApplicationType,
							CreatedAt = pr.CreatedAt,
							PlannedAt = pr.PlannedAt,
							EquipmentType = pr.EquipmentType,
							EquipmentKind = pr.EquipmentKind,
							EquipmentModel = pr.EquipmentModel,
							SerialNumber = pr.SerialNumber,
							TypeBreakdown = pr.TypeBreakdown,
							DescriptionMalfunction = pr.DescriptionMalfunction,
							ApplicationStatus = pr.ApplicationStatus,
							InternalComment = pr.InternalComment,
							CompletionAt = pr.CompletionAt,

							CustomerEmployee = new EmployeeDataModel
							{
								Id = pr.CustomerEmployee.Id,
								Account = pr.CustomerEmployee.Account,
								Role = pr.CustomerEmployee.Role,
								Name = pr.CustomerEmployee.Name,
								Department = pr.CustomerEmployee.Department,
								Email = pr.CustomerEmployee.Email,
								Phone = pr.CustomerEmployee.Phone,
								Specialized = pr.CustomerEmployee.Specialized
							},

							ExecutorEmployee = new EmployeeDataModel
							{
								Id = pr.ExecutorEmployee.Id,
								Account = pr.ExecutorEmployee.Account,
								Role = pr.ExecutorEmployee.Role,
								Name = pr.ExecutorEmployee.Name,
								Department = pr.ExecutorEmployee.Department,
								Email = pr.ExecutorEmployee.Email,
								Phone = pr.ExecutorEmployee.Phone,
								Specialized = pr.ExecutorEmployee.Specialized
							}
						});

					return Results.Ok(processRequests);
				});

			processRequestGroup.MapPost("UpdateProcessRequest",
				[Authorize]
			async ([FromBody] UpdateProcessRequestModel updateProcess, IMediator mediator, CancellationToken token) =>
			{
				if (updateProcess is null)
					return Results.BadRequest("Command cannot be null");

				UpdateProcessRequestCommand? command = new()
				{
					Id = updateProcess.Id,
					ApplicationType = updateProcess.ApplicationType,
					EquipmentType = updateProcess.EquipmentType,
					EquipmentKind = updateProcess.EquipmentKind,
					EquipmentModel = updateProcess.EquipmentModel,
					SerialNumber = updateProcess.SerialNumber,
					TypeBreakdown = updateProcess.TypeBreakdown,
					DescriptionMalfunction = updateProcess.DescriptionMalfunction,
					ApplicationStatus = updateProcess.ApplicationStatus,
					InternalComment = updateProcess.InternalComment,
					CustomerEmployeeId = updateProcess.CustomerEmployeeId,
					ApplicationNumber = updateProcess.ApplicationNumber,
					CreatedAt = updateProcess.CreatedAt,
					PlannedAt = updateProcess.PlannedAt
				};

				bool result = await mediator.Send(command, token);

				return result ? Results.Ok(result) : Results.BadRequest("Failed to update process request");
			});

			processRequestGroup.MapPost("UpdateStatusDoneOrRejected",
				[Authorize]
			async ([FromBody] UpdateStatusDoneOrRejectedModel updateStatus, IMediator mediator, CancellationToken token) =>
			{
				if (updateStatus is null)
					return Results.BadRequest("Command cannot be null");

				UpdateStatusDoneOrRejectedCommand? command = new()
				{
					Id = updateStatus.Id,
					ApplicationStatus = updateStatus.ApplicationStatus,
					InternalComment = updateStatus.InternalComment,
					CompletedAt = updateStatus.CompletedAt
				};

				bool result = await mediator.Send(command, token);

				return result ? Results.Ok(result) : Results.BadRequest("Failed to update process request status");
			});

			processRequestGroup.MapPost("UpdateStatusInWorkOrReturnedOrPostponedRequest",
				[Authorize]
			async ([FromBody] UpdateStatusInWorkOrReturnedOrPostponedRequestModel updateStatus, IMediator mediator, CancellationToken token) =>
			{
				if (updateStatus is null)
					return Results.BadRequest("Command cannot be null");

				UpdateStatusInWorkOrReturnedOrPostponedRequestCommand? command = new()
				{
					Id = updateStatus.Id,
					ApplicationStatus = updateStatus.ApplicationStatus,
					InternalComment = updateStatus.InternalComment
				};

				bool result = await mediator.Send(command, token);

				return result ? Results.Ok(result) : Results.BadRequest("Failed to update process request status");
			});

			processRequestGroup.MapPost("UpdateStatusRedirectedRequest",
				[Authorize]
			async ([FromBody] UpdateStatusRedirectedRequestModel updateStatus, IMediator mediator, CancellationToken token) =>
			{
				if (updateStatus is null)
					return Results.BadRequest("Command cannot be null");

				UpdateStatusRedirectedRequestCommand? command = new()
				{
					Id = updateStatus.Id,
					ApplicationStatus = updateStatus.ApplicationStatus,
					InternalComment = updateStatus.InternalComment,
					ExecutorEmployeeId = updateStatus.ExecutorEmployeeId
				};

				bool result = await mediator.Send(command, token);

				return result ? Results.Ok(result) : Results.BadRequest("Failed to update process request status");
			});


			return app;
		}
	}
}
