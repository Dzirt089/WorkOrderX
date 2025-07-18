﻿using WorkOrderX.Http.Models;

namespace WorkOrderX.ApiClients.Employees.Interfaces
{

	public interface IEmployeeApiService : IWorkOrderXApi
	{
		Task<LoginResponseDataModel> LoginAsync(string account, CancellationToken token = default);
	}
}
