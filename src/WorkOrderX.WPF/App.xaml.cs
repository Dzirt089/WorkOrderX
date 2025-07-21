using AutoMapper;

using MailerVKT;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Windows;

using WorkOrderX.WPF.InternalServices;
using WorkOrderX.WPF.ViewModel;
using WorkOrderX.WPF.Views;

namespace WorkOrderX.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static IHost Host { get; private set; }

		public App()
		{
			Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					ConfigureServices(services);
				}).Build();
		}

		private static void ConfigureServices(IServiceCollection services)
		{
			try
			{

				services.AddCustomHttpClient()
						.AddJsonOptions()
						.AddServices();

				services.AddSingleton<NewRequestRepairViewModel>();
				services.AddSingleton<MainViewModel>();
				services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			}
			catch (Exception ex)
			{

			}
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			try
			{
				await Host.StartAsync();

				// Обработчик исключений UI-потока
				DispatcherUnhandledException += App_DispatcherUnhandledException;

				// Обработчик исключений в фоновых потоках
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

				// Обработчик необработанных исключений в Task
				TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

				var mapper = Host.Services.GetRequiredService<IMapper>();
				// Проверка валидности
				mapper.ConfigurationProvider.AssertConfigurationIsValid();

				var mainVM = Host.Services.GetRequiredService<MainViewModel>();
				await mainVM.InitializationAsync();
				new MainWindow { DataContext = mainVM }.Show();


				base.OnStartup(e);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Обработчик необработанных исключений в Task
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			HandleException(e.Exception);
			e.SetObserved(); // Помечаем исключение как обработанное
		}

		/// <summary>
		/// Обработчик необработанных исключений в AppDomain
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			HandleException((Exception)e.ExceptionObject);
		}

		/// <summary>
		/// Обработчик исключений UI-потока
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			HandleException(e.Exception);
			e.Handled = true; // Предотвращаем крах приложения
		}

		/// <summary>
		/// Обработчик исключений с выводом сообщения пользователю и отправкой на почту
		/// </summary>
		/// <param name="ex"></param>
		private void HandleException(Exception ex)
		{
			try
			{
				var mail = Host.Services.GetRequiredService<Sender>();
				mail.SendAsync(new MailParameters
				{
					Text = TextMail(ex),
					Recipients = ["teho19@vkt-vent.ru" /*, "teho12@vkt-vent.ru"*/],
					Subject = "Errors in WorkOrderX.WPF",
					SenderName = "WorkOrderX.WPF",
				}).ConfigureAwait(false);

				MessageBox.Show(
				"Произошла критическая ошибка.",
				"Основная ошибка: " + ex.Message,
				MessageBoxButton.OK,
				MessageBoxImage.Error);
			}
			catch (Exception e)
			{
				// Показ сообщения пользователю
				MessageBox.Show(
				"Произошла критическая ошибка.",
				"Основная ошибка: " + ex.Message + ".\n\nВторая ошибка :" + e.Message,
				MessageBoxButton.OK,
				MessageBoxImage.Error);
			}
		}


		protected override void OnExit(ExitEventArgs e)
		{
			Host.StopAsync();
			base.OnExit(e);
		}

		private static string TextMail(Exception ex)
		{
			return $@"
<pre>
WorkOrderX.WPF,
Время: {DateTime.Now},
Глобальная обработка исключений.


Сводка об ошибке: 

Message: {ex.Message}.


StackTrace: {ex.StackTrace}.


Source: {ex.Source}.


InnerException: {ex?.InnerException}.

</pre>";
		}
	}

}
