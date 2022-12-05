using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.APP.Services;
using PointCloudRenderer.Data;
using Microsoft.Win32;

namespace PointCloudRenderer.APP.ViewModels;

public sealed partial class MainWindowViewModel : BaseViewModel
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
			OnPropertyChanged();
		}
	}

	private readonly OpenFileDialog ofd = new();
	private readonly LoadPointCloudService loadPointCloudService;

	public MainWindowViewModel(SimplePointCloudScene scene, LoadPointCloudService loadPointCloudService)
	{
		Scene = scene;
		this.loadPointCloudService = loadPointCloudService;
	}

	public void Zoom(int delta)
	{
		Scene.ZoomLevel = Math.Max(0, Scene.ZoomLevel - (float)delta / 10000);
	}

	public void LoadCloud(PointCloud cloud)
	{
		Scene.Load(cloud);
		PointSize = 0.05f;
	}

	[RelayCommand]
	public void OpenFile()
	{
		ofd.Filter = ".xyz|*.xyz";
		ofd.Multiselect = false;

		if (ofd.ShowDialog() == true)
		{
			var cloud = loadPointCloudService.Load(ofd.FileName);
			if (cloud is not null)
				LoadCloud(cloud);
		}
	}
}
