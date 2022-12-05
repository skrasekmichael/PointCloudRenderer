using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.APP.Services;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
	public RelayCommand OpenFileCommand { get; }

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

	private readonly OpenFileDialog ofd = new();
	private readonly LoadPointCloudService loadPointCloudService;

	public MainWindowViewModel(SimplePointCloudScene scene, LoadPointCloudService loadPointCloudService)
	{
		Scene = scene;
		this.loadPointCloudService = loadPointCloudService;

		OpenFileCommand = new RelayCommand(OpenFile);
	}

	public void Zoom(int delta)
	{
		ZoomLevel = Math.Max(0, ZoomLevel - (float)delta / 10000);
	}

	public void LoadCloud(PointCloud cloud)
	{
		Scene.Load(cloud);
		PointSize = 0.1f;
	}

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
