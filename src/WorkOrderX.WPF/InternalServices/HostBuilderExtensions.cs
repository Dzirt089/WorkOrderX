using MailerVKT;

using Microsoft.Extensions.DependencyInjection;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

using WorkOrderX.ApiClients.Employees.Implementation;
using WorkOrderX.ApiClients.Employees.Interfaces;
using WorkOrderX.ApiClients.ProcessRequest.Implementation;
using WorkOrderX.ApiClients.ProcessRequest.Interfaces;
using WorkOrderX.ApiClients.ReferenceData.Implementation;
using WorkOrderX.ApiClients.ReferenceData.Interfaces;
using WorkOrderX.WPF.Models.Model.Global;
using WorkOrderX.WPF.Services.Implementation;
using WorkOrderX.WPF.Services.Interfaces;
using WorkOrderX.WPF.ViewModel;

namespace WorkOrderX.WPF.InternalServices
{
	/// <summary>
	/// Класс расширений для HostBuilder, содержащий методы для регистрации сервисов и HTTP-клиента.
	/// </summary>
	public static class HostBuilderExtensions
	{
		/// <summary>
		/// Метод для регистрации HTTP-клиента с настройками в контейнере зависимостей.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
		{
			// Регистрация обработчика заголовка авторизации
			services.AddScoped<AuthHeaderHandler>();

			// Регистрация HTTP-клиента для API "WorkOrderXApi" с базовым адресом и настройками заголовков
			services.AddHttpClient("WorkOrderXApi", _ =>
			{
				_.BaseAddress = new Uri(Settings.Default.WorkOrderXApi); // Базовый адрес API
				_.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // Установка заголовка Accept для JSON
			})
			.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
			{
				PooledConnectionIdleTimeout = TimeSpan.FromSeconds(60), // Таймаут для неактивных подключений
				MaxConnectionsPerServer = 20, // Максимальное количество подключений к серверу
				AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Brotli, // Автоматическая декомпрессия ответов
				AllowAutoRedirect = false // Отключение автоматического перенаправления
			})
			.AddHttpMessageHandler<AuthHeaderHandler>(); // Добавление обработчика заголовка авторизации

			return services;
		}

		/// <summary>
		/// Метод для регистрации настроек сериализации JSON в контейнере зависимостей.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddJsonOptions(this IServiceCollection services)
		{
			// Это позволяет сериализовать и десериализовать объекты с циклическими ссылками
			services.AddSingleton(new JsonSerializerOptions
			{
				ReferenceHandler = ReferenceHandler.Preserve, // Обработка циклических ссылок
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Преобразует имена свойств в camelCase SomeProperty → someProperty
				MaxDepth = 2048, // Устанавливает максимальную глубину вложенности объектов (защита от переполнения стека)
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull //Игнорирование null-значений
			});
			return services;
		}

		/// <summary>
		/// Метод для регистрации сервисов в контейнере зависимостей.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IEmployeeApiService, EmployeeApiService>();
			services.AddScoped<IReferenceDataApiService, ReferenceDataApiService>();
			services.AddScoped<IProcessRequestApiService, ProcessRequestApiService>();

			services.AddScoped<IReferenceDadaServices, ReferenceDadaServices>();
			services.AddScoped<IProcessRequestService, ProcessRequestService>();
			services.AddScoped<IEmployeeService, EmployeeService>();

			services.AddSingleton<Sender>();
			services.AddSingleton<GlobalEmployeeForApp>();
			services.AddSingleton<INavigationService, NavigationService>();

			services.AddSingleton<NewRequestRepairViewModel>();
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<ActiveRequestViewModel>();
			services.AddSingleton<SelectRequestRepairViewModel>();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			services.AddMemoryCache();

			return services;
		}
	}
}
