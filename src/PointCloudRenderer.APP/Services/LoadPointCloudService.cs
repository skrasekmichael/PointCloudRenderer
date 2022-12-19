using Microsoft.Extensions.DependencyInjection;
using PointCloudRenderer.APP.Views;
using PointCloudRenderer.Data;
using System.IO;

namespace PointCloudRenderer.APP.Services;

public sealed class LoadPointCloudService
{
	private readonly IServiceProvider serviceProvider;
	private readonly OpenedRecentlyService openedRecentlyService;

	public LoadPointCloudService(IServiceProvider serviceProvider, OpenedRecentlyService openedRecentlyService)
	{
		this.serviceProvider = serviceProvider;
		this.openedRecentlyService = openedRecentlyService;
	}

	public async Task<PointCloud?> LoadAsync(string path)
	{
		path = Path.GetFullPath(path);
		var window = serviceProvider.GetRequiredService<LoadPointCloudWindow>();
		await window.ViewModel.LoadAsync(path);

		if (window.ShowDialog() == true)
		{
			await openedRecentlyService.AddFileAsync(path);
			return window.ViewModel.Cloud;
		}

		return null;
	}
}
