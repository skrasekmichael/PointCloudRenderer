using Microsoft.Extensions.DependencyInjection;
using PointCloudRenderer.APP.Views;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.Services;

public class LoadPointCloudService
{
	private readonly IServiceProvider serviceProvider;

	public LoadPointCloudService(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public PointCloud? Load(string path)
	{
		var window = serviceProvider.GetRequiredService<LoadPointCloudWindow>();
		window.ViewModel.Load(path);

		if (window.ShowDialog() == true)
			return window.ViewModel.Cloud;

		return null;
	}
}
