using MailerVKT;

using MediatR;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Application.Handlers.DomainEventHandler;
using WorkOrderX.Application.Queries.GetEmployeeByAccount;
using WorkOrderX.Application.Queries.GetEmployeeByAccount.Responses;
using WorkOrderX.Http.Models;

namespace WorkOrderX.API
{
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// Добавляет MediatR в сервисы приложения.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddInfrastructureMediatR(this IServiceCollection services)
		{
			services.AddMediatR(cfg =>
				cfg.RegisterServicesFromAssembly(typeof(ProcessRequestStatusChangedDomainEventHandler).Assembly));
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
		/// Добавляет маршруты для получения информации о сотруднике по его учетной записи.
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static WebApplication AddInfrastructureMapEmployee(this WebApplication app)
		{
			var employeeGroup = app.MapGroup("Employee");
			employeeGroup.MapGet("GetEmployeeByAccount/{account}", async (string account, IMediator mediator) =>
			{
				GetEmployeeByAccountQuery? query = new GetEmployeeByAccountQuery { UserAccount = account };
				GetEmployeeByAccountQueryResponse? response = await mediator.Send(query);
				var result = new EmployeeDataModel
				{
					Account = response.EmployeeDataDto.Account,
					Role = response.EmployeeDataDto.Role,
					Name = response.EmployeeDataDto.Name,
					Department = response.EmployeeDataDto.Department,
					Email = response.EmployeeDataDto.Email,
					Phone = response.EmployeeDataDto.Phone,
					Specialized = response.EmployeeDataDto.Specialized
				};

				return Results.Ok(result);
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
			var processRequestGroup = app.MapGroup("ProcessRequest");
			processRequestGroup.MapPost("CreateProcessRequest", async (CreateProcessRequestModel createProcess, IMediator mediator) =>
			{
				if (createProcess is null)
				{
					return Results.BadRequest("Command cannot be null");
				}

				CreateProcessRequestCommand? command = new CreateProcessRequestCommand
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

				bool result = await mediator.Send(command);
				return result ? Results.Ok(result) : Results.BadRequest("Failed to create process request");
			});

			processRequestGroup.MapPost("UpdateProcessRequest", async (UpdateProcessRequestModel updateProcess, IMediator mediator) =>
			{
				if (updateProcess is null)
				{
					return Results.BadRequest("Command cannot be null");
				}
				UpdateProcessRequestCommand? command = new UpdateProcessRequestCommand
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
				bool result = await mediator.Send(command);
				return result ? Results.Ok(result) : Results.BadRequest("Failed to update process request");
			});

			processRequestGroup.MapPost("UpdateStatusDoneOrRejected", async (UpdateStatusDoneOrRejectedModel updateStatus, IMediator mediator) =>
			{
				if (updateStatus is null)
				{
					return Results.BadRequest("Command cannot be null");
				}

				UpdateStatusDoneOrRejectedCommand? command = new UpdateStatusDoneOrRejectedCommand
				{
					Id = updateStatus.Id,
					ApplicationStatus = updateStatus.ApplicationStatus,
					InternalComment = updateStatus.InternalComment,
					CompletedAt = updateStatus.CompletedAt
				};

				bool result = await mediator.Send(command);
				return result ? Results.Ok(result) : Results.BadRequest("Failed to update process request status");
			});

			processRequestGroup.MapPost("UpdateStatusInWorkOrReturnedOrPostponedRequest", async (UpdateStatusInWorkOrReturnedOrPostponedRequestModel updateStatus, IMediator mediator) =>
			{
				if (updateStatus is null)
				{
					return Results.BadRequest("Command cannot be null");
				}

				UpdateStatusInWorkOrReturnedOrPostponedRequestCommand? command = new UpdateStatusInWorkOrReturnedOrPostponedRequestCommand
				{
					Id = updateStatus.Id,
					ApplicationStatus = updateStatus.ApplicationStatus,
					InternalComment = updateStatus.InternalComment
				};
				bool result = await mediator.Send(command);
				return result ? Results.Ok(result) : Results.BadRequest("Failed to update process request status");
			});

			processRequestGroup.MapPost("UpdateStatusRedirectedRequest", async (UpdateStatusRedirectedRequestModel updateStatus, IMediator mediator) =>
			{
				if (updateStatus is null)
				{
					return Results.BadRequest("Command cannot be null");
				}
				UpdateStatusRedirectedRequestCommand? command = new UpdateStatusRedirectedRequestCommand
				{
					Id = updateStatus.Id,
					ApplicationStatus = updateStatus.ApplicationStatus,
					InternalComment = updateStatus.InternalComment,
					ExecutorEmployeeId = updateStatus.ExecutorEmployeeId
				};
				bool result = await mediator.Send(command);
				return result ? Results.Ok(result) : Results.BadRequest("Failed to update process request status");
			});


			return app;
		}
	}
}
