using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.APP.ViewModels;
using PointCloudRenderer.APP.Views;
using System.Windows;

namespace PointCloudRenderer.APP;

public partial class App : Application
{
	private readonly IHost host;

	public App()
	{
		host = Host.CreateDefaultBuilder()
			.ConfigureAppConfiguration(ConfigureAppConfiguration)
			.ConfigureServices((context, services) => ConfigureServices(context.Configuration, services))
			.Build();
	}

	private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
	{
	}

	private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
	{
		services.AddSingleton<MainWindow>();

		//view models
		services.AddSingleton<MainWindowViewModel>();

		//scenes
		services.AddSingleton<SimplePointCloudScene>();
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		await host.StartAsync();

		var viewModel = host.Services.GetRequiredService<MainWindowViewModel>();
		var window = new MainWindow(viewModel);

		viewModel.LoadCloud(e.Args[0]);

		window.Show();

		base.OnStartup(e);
	}

	protected override async void OnExit(ExitEventArgs e)
	{
		using (host)
		{
			await host.StopAsync(TimeSpan.FromSeconds(5));
		}

		base.OnExit(e);
	}
}
