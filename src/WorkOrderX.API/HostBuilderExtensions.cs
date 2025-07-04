using MailerVKT;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using WorkOrderX.Application.Commands.ProcessRequest;
using WorkOrderX.Application.Handlers.DomainEventHandler;
using WorkOrderX.Application.Queries.GetEmployeeByAccount;
using WorkOrderX.Application.Queries.GetEmployeeByAccount.Responses;
using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById;
using WorkOrderX.Application.Queries.GetProcessRequestFromCustomerById.Responses;
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
			employeeGroup.MapGet("GetEmployeeByAccount/{account}",
				async (string account, IMediator mediator, CancellationToken token) =>
			{
				if (string.IsNullOrEmpty(account))
					return Results.BadRequest("Account cannot be null or empty");

				GetEmployeeByAccountQuery? query = new() { UserAccount = account };
				GetEmployeeByAccountQueryResponse? response = await mediator.Send(query, token);
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
			processRequestGroup.MapPost("CreateProcessRequest",
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

			processRequestGroup.MapPost("GetActivProcessRequests",
				async ([FromBody] GetActivProcessRequestFromCustomerByIdModel byIdModel, IMediator mediator, CancellationToken token) =>
			{
				if (byIdModel is null || byIdModel.Id == Guid.Empty)
					return Results.BadRequest("Command cannot be null");

				var query = new GetActivProcessRequestFromCustomerByIdQuery { Id = byIdModel.Id };
				GetActivProcessRequestFromCustomerByIdQueryResponse? response = await mediator.Send(query, token);

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
