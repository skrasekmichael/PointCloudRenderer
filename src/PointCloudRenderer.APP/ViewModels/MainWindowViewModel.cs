using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.APP.Services;
using PointCloudRenderer.Data;
using Microsoft.Win32;

namespace PointCloudRenderer.APP.ViewModels;

public sealed partial class MainWindowViewModel : BaseViewModel
{
	public SimplePointCloudScene Scene { get; }

	private float _pointSize = 0.07f;
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
		Scene.ZoomLevel = Math.Max(0, Scene.ZoomLevel - delta * 0.0001f);
	}

	public void LoadCloud(PointCloud cloud)
	{
		Scene.Load(cloud);
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
