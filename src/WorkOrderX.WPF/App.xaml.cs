using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Windows;

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
			//services.AddSingleton<IProductService, ProductService>();
			//services.AddSingleton<INavigationService, NavigationService>();
			//services.AddSingleton<ShopViewModel>();
			//services.AddSingleton<CartViewModel>();
			//services.AddSingleton<ProfileViewModel>();
			services.AddSingleton<MainViewModel>();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
				var mainVM = Host.Services.GetRequiredService<MainViewModel>();
				new MainWindow { DataContext = mainVM }.Show();
				base.OnStartup(e);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}

}
