using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Services;
using Microsoft.Win32;

namespace PointCloudRenderer.APP.ViewModels;

public sealed partial class MainWindowViewModel : BaseViewModel
{
	private readonly OpenFileDialog ofd = new();
	private readonly LoadPointCloudService loadPointCloudService;

	private readonly SimplePointCloudSceneViewModel simplePointCloudSceneViewModel;

	public BaseSceneViewModel SceneViewModel { get; set; }

	public MainWindowViewModel(
		SimplePointCloudSceneViewModel simplePointCloudSceneViewModel,
		LoadPointCloudService loadPointCloudService)
	{
		this.simplePointCloudSceneViewModel = simplePointCloudSceneViewModel;
		this.loadPointCloudService = loadPointCloudService;
		SceneViewModel = simplePointCloudSceneViewModel;
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
				simplePointCloudSceneViewModel.LoadCloud(cloud);
		}
	}
}
