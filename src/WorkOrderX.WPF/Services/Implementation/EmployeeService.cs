using AutoMapper;

using WorkOrderX.ApiClients.Employees.Interfaces;
using WorkOrderX.WPF.Models.Model;
using WorkOrderX.WPF.Models.Model.Base;
using WorkOrderX.WPF.Services.Interfaces;

namespace WorkOrderX.WPF.Services.Implementation
{
	/// <summary>
	/// Сервис для работы с сотрудниками.
	/// </summary>
	public class EmployeeService : ViewModelBase, IEmployeeService
	{
		private readonly IMapper _mapper;
		private readonly IEmployeeApiService _employeeService;

		public EmployeeService(IMapper mapper, IEmployeeApiService employeeService)
		{
			_mapper = mapper;
			_employeeService = employeeService;
		}

		/// <summary>
		/// Получение сотрудников по роли.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ApplicationException"></exception>
		public async Task<IEnumerable<Employee>> GetByRoleEmployeesAsync(string role, CancellationToken token = default)
		{
			if (string.IsNullOrEmpty(role))
			{
				throw new ArgumentException("Role cannot be null or empty.", nameof(role));
			}

			// Получаем сотрудников по роли через API
			var employeeDto = await _employeeService.GetByRoleEmployeesAsync("Executer", token);

			// Проверяем, что данные получены
			if (employeeDto == null || !employeeDto.Any())
			{
				throw new ApplicationException($"Employee not found for role: {role}");
			}
			// Преобразуем DTO в модели
			return _mapper.Map<IEnumerable<Employee>>(employeeDto);
		}
	}
}
