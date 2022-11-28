using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		await host.StartAsync();

		var window = host.Services.GetRequiredService<MainWindow>();
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
