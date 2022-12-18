using Microsoft.Extensions.DependencyInjection;
using PointCloudRenderer.APP.Views;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.Services;

public sealed class LoadPointCloudService
{
	private readonly IServiceProvider serviceProvider;

	public LoadPointCloudService(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public async Task<PointCloud?> LoadAsync(string path)
	{
		var window = serviceProvider.GetRequiredService<LoadPointCloudWindow>();
		await window.ViewModel.LoadAsync(path);

		if (window.ShowDialog() == true)
			return window.ViewModel.Cloud;

		return null;
	}
}
