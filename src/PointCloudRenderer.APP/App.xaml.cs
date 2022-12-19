using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.APP.Services;
using PointCloudRenderer.APP.ViewModels;
using PointCloudRenderer.APP.Views;
using System.Windows;

namespace PointCloudRenderer.APP;

public sealed partial class App : Application
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
		//views and windows
		services.AddSingleton<MainWindow>();
		services.AddTransient<LoadPointCloudWindow>();

		//view models
		services.AddSingleton<MainWindowViewModel>();
		services.AddSingleton<LoadPointCloudWindowViewModel>();
		services.AddSingleton<SimplePointCloudSceneViewModel>();

		//scenes
		services.AddSingleton<SimplePointCloudScene>();

		//services
		services.AddSingleton<LoadPointCloudService>();
		services.AddSingleton<OpenedRecentlyService>();
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		await host.StartAsync();

		var openedRecentlyService = host.Services.GetRequiredService<OpenedRecentlyService>();
		await openedRecentlyService.LoadAsync();

		var window = host.Services.GetRequiredService<MainWindow>();

		if (e.Args.Length > 0)
		{
			var loader = host.Services.GetRequiredService<LoadPointCloudService>();
			var cloud = await loader.LoadAsync(e.Args[0]);

			if (cloud is null)
				Environment.Exit(0);

			(window.DataContext as MainWindowViewModel)!.Cloud = cloud;
		}

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
