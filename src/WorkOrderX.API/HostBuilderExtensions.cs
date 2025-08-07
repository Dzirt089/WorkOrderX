using MailerVKT;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using WorkOrderX.API.ReferenceData;
using WorkOrderX.Application.Behaviors;
using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Application.Handlers.DomainEventHandler;
using WorkOrderX.Application.Hubs;
using WorkOrderX.Application.Queries.GetAllApplicationStatus;
using WorkOrderX.Application.Queries.GetAllApplicationType;
using WorkOrderX.Application.Queries.GetAllEquipmentKind;
using WorkOrderX.Application.Queries.GetAllEquipmentModel;
using WorkOrderX.Application.Queries.GetAllEquipmentType;
using WorkOrderX.Application.Queries.GetAllImportances;
using WorkOrderX.Application.Queries.GetAllTypeBreakdown;
using WorkOrderX.Application.Queries.GetByRoleEmployees;
using WorkOrderX.Application.Queries.GetEmployeeByAccount;
using WorkOrderX.Application.Queries.GetEmployeeByAccount.Responses;
using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById;
using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses;
using WorkOrderX.Domain.AggregationModels.ProcessRequests;
using WorkOrderX.Domain.AggregationModels.WorkplaceEmployees;
using WorkOrderX.Domain.Models.EventStores;
using WorkOrderX.DomainService.ProcessRequestServices.Implementation;
using WorkOrderX.DomainService.ProcessRequestServices.Interfaces;
using WorkOrderX.EFCoreDb.Configurations;
using WorkOrderX.EFCoreDb.DbContexts;
using WorkOrderX.Http.Models;
using WorkOrderX.Infrastructure.Repositories.Implementation;
using WorkOrderX.Infrastructure.Repositories.Interfaces;

namespace WorkOrderX.API
{
	public static class HostBuilderExtensions
	{
		public static IServiceCollection AddInfrastructureSignalR(this IServiceCollection services)
		{
			services.AddSignalR();
			return services;
		}

		public static WebApplication AddMapHubActiv(this WebApplication app)
		{
			app.MapHub<ProcessRequestChangedHub>("/RequestChanged");
			return app;
		}

		/// <summary>
		/// Добавляет настройки JSON и сжатия в сервисы приложения.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddInfrastructureJsonOptionsWithCompressions(this IServiceCollection services)
		{
			// Активирует middleware сжатия
			services.AddResponseCompression();

			//Настраиваем middleware сжатия
			services.Configure<ResponseCompressionOptions>(options =>
			{
				options.EnableForHttps = true;
				options.MimeTypes = new[] { "application/json" };
			});

			//Конфигурация Json, для получения из DI с использованием в HttpClient
			services.ConfigureHttpJsonOptions(options =>
			{
				options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
				options.SerializerOptions.MaxDepth = 2048;

				options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				options.SerializerOptions.WriteIndented = false;
				options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			});

			return services;
		}

		/// <summary>
		/// Добавляет MediatR в сервисы приложения.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddInfrastructureMediatRPipeline(this IServiceCollection services)
		{
			// Регистрация MediatR и обработчиков команд и запросов
			services.AddMediatR(cfg =>
				cfg.RegisterServicesFromAssembly(typeof(ProcessRequestStatusChangedDomainEventHandler).Assembly));

			// Регистрация поведения MediatR для обработки команд и запросов
			services.AddTransient(
				typeof(IPipelineBehavior<,>),
				typeof(DomainEventsDispatchingBehavior<,>));

			services.AddTransient(
				typeof(IPipelineBehavior<,>),
				typeof(UnitOfWorkBehavior<,>));

			services.AddTransient(
				typeof(IPipelineBehavior<,>),
				typeof(IntegrationEventsDispatchingBehavior<,>));

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
			var dbConfigSection = configuration.GetSection("ConnectionStrings");
			var dbConfig = dbConfigSection.Get<DbConfiguration>();

			services.Configure<DbConfiguration>(configuration);
			services.AddDbContext<WorkOrderDbContext>(_ =>
			{
				_.UseSqlServer(dbConfig.ConnectionString, _ =>

				//Задаем в коде максимальный уровень 110 совместимости (говорим EF Core, что он работает с SQL Server 2012)
				_.UseCompatibilityLevel(110));
			});

			return services;
		}

		/// <summary>
		/// Добавляет сервисы и репозитории в коллекцию сервисов приложения.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
		{
			// Агрегаты
			services.AddScoped<IWorkplaceEmployeesRepository, WorkplaceEmployeesRepository>();
			services.AddScoped<IProcessRequestRepository, ProcessRequestRepository>();
			services.AddScoped<IEventStoreEntryRepository, EventStoreEntryRepository>();
			// Справочники
			services.AddScoped<IReferenceDataRepository<EquipmentType>, ReferenceDataRepository<EquipmentType>>();
			services.AddScoped<IReferenceDataRepository<EquipmentKind>, ReferenceDataRepository<EquipmentKind>>();
			services.AddScoped<IReferenceDataRepository<TypeBreakdown>, ReferenceDataRepository<TypeBreakdown>>();
			services.AddScoped<IReferenceDataRepository<ApplicationStatus>, ReferenceDataRepository<ApplicationStatus>>();
			services.AddScoped<IReferenceDataRepository<ApplicationType>, ReferenceDataRepository<ApplicationType>>();
			services.AddScoped<IReferenceDataRepository<Role>, ReferenceDataRepository<Role>>();
			services.AddScoped<IReferenceDataRepository<Specialized>, ReferenceDataRepository<Specialized>>();
			services.AddScoped<IReferenceDataRepository<EquipmentModel>, ReferenceDataRepository<EquipmentModel>>();
			services.AddScoped<IReferenceDataRepository<Importance>, ReferenceDataRepository<Importance>>();
			// Доменный сервис
			services.AddScoped<IProcessRequestService, ProcessRequestService>();
			// Вспомогательный сервис
			services.AddScoped<IAppNumberRepository, AppNumberRepository>();

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

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, response.EmployeeDataDto.Name),
					new Claim(ClaimTypes.NameIdentifier, response.EmployeeDataDto.Id.ToString()),
					new Claim(ClaimTypes.Role, response.EmployeeDataDto.Role)
				};

				var jwt = new JwtSecurityToken(
					issuer: configuration["JwtSettings:Issuer"],
					audience: configuration["JwtSettings:Audience"],
					claims: claims,
					expires: DateTime.Now.AddDays(1),
					signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256)
					);


				return Results.Ok(new LoginResponseDataModel
				{
					Token = new JwtSecurityTokenHandler().WriteToken(jwt),
					Employee = result
				});
			});

			employeeGroup.MapGet("GetByRoleEmployees/{role}",
				[Authorize]
			async (string role, IMediator mediator, CancellationToken token) =>
			{
				if (string.IsNullOrEmpty(role))
					return Results.BadRequest("Role cannot be null or empty");

				var query = new GetByRoleEmployeesQuery { Role = role };
				var response = await mediator.Send(query, token);

				if (response.Employees is null || !response.Employees.Any())
					return Results.Ok(new List<EmployeeDataModel>());

				return Results.Ok(
					response.Employees.Select(e => new EmployeeDataModel
					{
						Id = e.Id,
						Account = e.Account,
						Role = e.Role,
						Name = e.Name,
						Department = e.Department,
						Email = e.Email,
						Phone = e.Phone,
						Specialized = e.Specialized
					}));
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

				CreateProcessRequestCommand? command = new CreateProcessRequestCommand()
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
					Location = createProcess.Location,
					CustomerEmployeeId = createProcess.CustomerEmployeeId,
					Importance = createProcess.Importance,
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

				if (response.ProcessRequests is null || !response.ProcessRequests.Any())
					return Results.Ok(new List<ProcessRequestDataModel>());

				IEnumerable<ProcessRequestDataModel> processRequests = response.ProcessRequests
					.Select(pr => new ProcessRequestDataModel
					{
						Id = pr.Id,
						ApplicationNumber = pr.ApplicationNumber,
						ApplicationType = pr.ApplicationType,
						CreatedAt = pr.CreatedAt,
						PlannedAt = pr.PlannedAt,
						UpdatedAt = pr.UpdatedAt,
						EquipmentType = pr.EquipmentType,
						EquipmentKind = pr.EquipmentKind,
						EquipmentModel = pr.EquipmentModel,
						SerialNumber = pr.SerialNumber,
						TypeBreakdown = pr.TypeBreakdown,
						DescriptionMalfunction = pr.DescriptionMalfunction,
						ApplicationStatus = pr.ApplicationStatus,
						InternalComment = pr.InternalComment,
						CompletionAt = pr.CompletionAt,
						Importance = pr.Importance,
						Location = pr.Location,

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

					if (response.ProcessRequests is null || !response.ProcessRequests.Any())
						return Results.Ok(new List<ProcessRequestDataModel>());

					IEnumerable<ProcessRequestDataModel> processRequests = response.ProcessRequests
						.Select(pr => new ProcessRequestDataModel
						{
							Id = pr.Id,
							ApplicationNumber = pr.ApplicationNumber,
							ApplicationType = pr.ApplicationType,
							CreatedAt = pr.CreatedAt,
							UpdatedAt = pr.UpdatedAt,
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
							Importance = pr.Importance,
							Location = pr.Location,

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
					PlannedAt = updateProcess.PlannedAt,
					Importance = updateProcess.Importance,
					Location = updateProcess.Location,
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
					InternalComment = updateStatus.InternalComment,
					PlannedAt = updateStatus.PlannedAt
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


			processRequestGroup.MapPost("UpdateInternalCommentRequest",
				[Authorize]
			async ([FromBody] UpdateInternalCommentRequestModel updateComment, IMediator mediator, CancellationToken token) =>
			{
				if (updateComment is null)
					return Results.BadRequest("Command cannot be null");

				UpdateInternalCommentCommand? command = new()
				{
					Id = updateComment.Id,
					InternalComment = updateComment.InternalComment
				};

				bool _ = await mediator.Send(command, token);

				return _ ? Results.Ok(_) : Results.BadRequest("Failed to update process request InternalComment");
			});

			return app;
		}

		/// <summary>
		/// Добавляет маршруты для получения справочных данных заявок.
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication AddInfrastructureReferenceDatas(this WebApplication app)
		{
			var refDatas = app.MapGroup("ReferenceData").RequireAuthorization();

			//ApplicationStatus
			refDatas.MapGet("GetAllApplicationStatus",
				[Authorize]
			async (IMediator mediator, CancellationToken token) =>
			{
				GetAllApplicationStatusQuery querys = new();
				var response = await mediator.Send(querys, token);

				IEnumerable<ApplicationStatusDataModel> datas = response.ApplicationStatusDatas
					.Select(_ => new ApplicationStatusDataModel
					{
						Id = _.Id,
						Name = _.Name,
						Description = _.Description
					});

				return Results.Ok(datas);
			});

			//ApplicationType
			refDatas.MapGet("GetAllApplicationType",
				[Authorize]
			async (IMediator mediator, CancellationToken token) =>
				{
					GetAllApplicationTypeQuery querys = new();
					var response = await mediator.Send(querys, token);

					IEnumerable<ApplicationTypeDataModel> datas = response.ApplicationTypeDatas
						.Select(_ => new ApplicationTypeDataModel
						{
							Id = _.Id,
							Name = _.Name,
							Description = _.Description
						});

					return Results.Ok(datas);
				});

			//EquipmentKind
			refDatas.MapGet("GetAllEquipmentKind",
				[Authorize]
			async (IMediator mediator, CancellationToken token) =>
				{
					GetAllEquipmentKindQuery querys = new();
					var response = await mediator.Send(querys, token);

					IEnumerable<EquipmentKindDataModel> datas = response.EquipmentKindDatas
						.Select(_ => new EquipmentKindDataModel
						{
							Id = _.Id,
							Name = _.Name,
							Description = _.Description,
							Type = new EquipmentTypeDataModel
							{
								Id = _.Type.Id,
								Name = _.Type.Name,
								Description = _.Type.Description
							}
						});

					return Results.Ok(datas);
				});

			//EquipmentModel
			refDatas.MapGet("GetAllEquipmentModel",
				[Authorize]
			async (IMediator mediator, CancellationToken token) =>
				{
					GetAllEquipmentModelQuery querys = new();
					var response = await mediator.Send(querys, token);

					IEnumerable<EquipmentModelDataModel> datas = response.EquipmentModelDatas
						.Select(_ => new EquipmentModelDataModel
						{
							Id = _.Id,
							Name = _.Name,
							Description = _.Description
						});

					return Results.Ok(datas);
				});

			//EquipmentType
			refDatas.MapGet("GetAllEquipmentType",
				[Authorize]
			async (IMediator mediator, CancellationToken token) =>
				{
					GetAllEquipmentTypeQuery querys = new();
					var response = await mediator.Send(querys, token);

					IEnumerable<EquipmentTypeDataModel> datas = response.EquipmentTypeDatas
						.Select(_ => new EquipmentTypeDataModel
						{
							Id = _.Id,
							Name = _.Name,
							Description = _.Description
						});

					return Results.Ok(datas);
				});

			//TypeBreakdown
			refDatas.MapGet("GetAllTypeBreakdown",
				[Authorize]
			async (IMediator mediator, CancellationToken token) =>
				{
					GetAllTypeBreakdownQuery querys = new();
					var response = await mediator.Send(querys, token);

					IEnumerable<TypeBreakdownDataModel> datas = response.TypeBreakdownDatas
						.Select(_ => new TypeBreakdownDataModel
						{
							Id = _.Id,
							Name = _.Name,
							Description = _.Description,
							Type = new EquipmentTypeDataModel
							{
								Id = _.Type.Id,
								Name = _.Type.Name,
								Description = _.Type.Description
							}
						});

					return Results.Ok(datas);
				});

			//Importance
			refDatas.MapGet("GetAllImportances",
				[Authorize]
			async (IMediator mediator, CancellationToken token) =>
				{
					GetAllImportancesQuery querys = new();
					var response = await mediator.Send(querys, token);

					IEnumerable<ApplicationStatusDataModel> datas = response.ImportancesDataDtos
						.Select(_ => new ApplicationStatusDataModel
						{
							Id = _.Id,
							Name = _.Name,
							Description = _.Description
						});

					return Results.Ok(datas);
				});

			return app;
		}
	}
}
