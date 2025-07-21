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

namespace WorkOrderX.WPF.InternalServices
{
	public static class HostBuilderExtensions
	{
		public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
		{
			services.AddScoped<AuthHeaderHandler>();

			services.AddHttpClient("WorkOrderXApi", _ =>
			{
				_.BaseAddress = new Uri(Settings.Default.WorkOrderXApi);
				_.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			})
			.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
			{
				PooledConnectionIdleTimeout = TimeSpan.FromSeconds(30),
				MaxConnectionsPerServer = 20,
				AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Brotli,
				AllowAutoRedirect = false
			})
			.AddHttpMessageHandler<AuthHeaderHandler>();

			return services;
		}

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

		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IEmployeeApiService, EmployeeApiService>();
			services.AddScoped<IReferenceDataApiService, ReferenceDataApiService>();
			services.AddScoped<IReferenceDadaServices, ReferenceDadaServices>();
			services.AddScoped<IProcessRequestApiService, ProcessRequestApiService>();

			services.AddSingleton<Sender>();
			services.AddSingleton<GlobalEmployeeForApp>();
			services.AddSingleton<INavigationService, NavigationService>();

			return services;
		}
	}
}
