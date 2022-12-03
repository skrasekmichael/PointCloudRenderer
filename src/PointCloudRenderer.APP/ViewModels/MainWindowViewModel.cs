using Microsoft.Extensions.DependencyInjection;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.APP.Views;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
	public SimplePointCloudScene Scene { get; }

	private float _pointSize = 0;
	public float PointSize
	{
		get => _pointSize;
		set
		{
			_pointSize = value;
			Scene.SetPointSize(value);
		}
	}

	public float ZoomLevel
	{
		get => Scene.ZoomLevel;
		set
		{
			Scene.ZoomLevel = value;
			OnPropertyChanged(nameof(ZoomLevel));
		}
	}

	private readonly IServiceProvider serviceProvider;

	public MainWindowViewModel(
		SimplePointCloudScene scene,
		IServiceProvider serviceProvider)
	{
		Scene = scene;
		this.serviceProvider = serviceProvider;
	}

	public void Zoom(int delta)
	{
		ZoomLevel = Math.Max(0, ZoomLevel - (float)delta / 10000);
	}

	public void LoadCloud(string path)
	{
		var window = serviceProvider.GetRequiredService<LoadPointCloudWindow>();
		window.ViewModel.Load(path);

		if (window.ShowDialog() == true)
		{
			Scene.Load(window.ViewModel.Cloud!);
			PointSize = 0.1f;
		}
	}
}
