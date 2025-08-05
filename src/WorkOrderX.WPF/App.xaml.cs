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
		#region Настройка хоста приложения, регистрация сервисов 

		/// <summary>
		/// Статическое свойство для хранения экземпляра хоста приложения.
		/// </summary>
		public static IHost Host { get; private set; }

		/// <summary>
		/// Конструктор приложения, инициализирующий хост и регистрирующий сервисы.
		/// </summary>
		public App()
		{
			// Инициализация хоста приложения с использованием Microsoft.Extensions.Hosting
			Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					ConfigureServices(services); // Регистрация сервисов
				}).Build();
		}

		/// <summary>
		/// Метод для регистрации сервисов в контейнере зависимостей.
		/// </summary>
		/// <param name="services"></param>
		private static void ConfigureServices(IServiceCollection services)
		{
			try
			{
				services.AddCustomHttpClient() // Регистрация HTTP-клиента с настройками
						.AddJsonOptions() // Регистрация настроек сериализации JSON
						.AddServices(); // Регистрация пользовательских сервисов
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Метод, вызываемый при запуске приложения.
		/// </summary>
		/// <param name="e"></param>
		protected override async void OnStartup(StartupEventArgs e)
		{
			try
			{
				// Запуск хоста приложения
				await Host.StartAsync();

				// Обработчик исключений UI-потока
				DispatcherUnhandledException += App_DispatcherUnhandledException;

				// Обработчик исключений в фоновых потоках
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

				// Обработчик необработанных исключений в Task
				TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

				// Получаем сервис AutoMapper из контейнера зависимостей
				var mapper = Host.Services.GetRequiredService<IMapper>();

				// Проверка валидности конфигурации AutoMapper
				mapper.ConfigurationProvider.AssertConfigurationIsValid();

				// Получаем ViewModel главного окна приложения из контейнера зависимостей
				var mainVM = Host.Services.GetRequiredService<MainViewModel>();

				// Инициализация данных для главного окна приложения
				await mainVM.InitializationAsync();

				// Создание и отображение главного окна приложения, передача ViewModel в качестве DataContext
				new MainWindow { DataContext = mainVM }.Show();

				base.OnStartup(e);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Метод, вызываемый при выходе из приложения.
		/// </summary>
		/// <param name="e"></param>
		protected async override void OnExit(ExitEventArgs e)
		{
			await Host.StopAsync();
			base.OnExit(e);
		}

		#endregion

		#region Обработчики исключений, отправка сообщений об ошибках

		/// <summary>
		/// Обработчик необработанных исключений в Task
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			// Обработка исключения
			HandleException(e.Exception);
			// Отменяем исключение, чтобы оно не привело к завершению приложения
			e.SetObserved(); // Помечаем исключение как обработанное
		}

		/// <summary>
		/// Обработчик необработанных исключений в AppDomain
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			// Обработка исключения
			HandleException((Exception)e.ExceptionObject);
		}

		/// <summary>
		/// Обработчик исключений UI-потока
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			// Обработка исключения
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


				// Получаем сервис отправки почты и отправляем сообщение об ошибке
				var mail = Host.Services.GetRequiredService<Sender>();
				mail.SendAsync(new MailParameters
				{
					Text = TextMail(ex),
					Recipients = ["teho19@vkt-vent.ru"],
					RecipientsBcc = ["progto@vkt-vent.ru"],
					Subject = "Errors in WorkOrderX.WPF",
					SenderName = "WorkOrderX.WPF",
				}).ConfigureAwait(false);

				// Показ сообщения пользователю
				MessageBox.Show(
					"Произошла ошибка в работе приложения. Сообщите разработчикам в ТО.",
					"Произошла критическая ошибка.",
				MessageBoxButton.OK,
				MessageBoxImage.Error);
			}
			catch (Exception e)
			{
				// Показ сообщения пользователю
				MessageBox.Show(
					"Произошла вторая ошибка в работе приложения. Сообщите разработчикам в ТО.",
					"Произошла критическая ошибка.",
				MessageBoxButton.OK,
				MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Метод для формирования текста письма с информацией об исключении.
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
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
		#endregion
	}

}
