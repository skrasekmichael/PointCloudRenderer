﻿using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Services;
using PointCloudRenderer.APP.Helpers;
using PointCloudRenderer.Data;
using Microsoft.Win32;
using System.Windows;

namespace PointCloudRenderer.APP.ViewModels;

public sealed partial class MainWindowViewModel : BaseViewModel
{
	private readonly OpenFileDialog ofd = new();
	private readonly LoadPointCloudService loadPointCloudService;

	public List<NamedObject<BaseSceneViewModel>> SceneTypes { get; } = new();

	private NamedObject<BaseSceneViewModel>? _namedSceneViewModel;
	public NamedObject<BaseSceneViewModel>? NamedSceneViewModel
	{
		get => _namedSceneViewModel;
		set
		{
			_namedSceneViewModel = value;
			if (Cloud is not null)
				NamedSceneViewModel?.Object.LoadCloud(Cloud);
			OnPropertyChanged();
		}
	}

	private PointCloud? _cloud;
	public PointCloud? Cloud
	{
		get => _cloud;
		set
		{
			_cloud = value;
			if (value is not null)
				NamedSceneViewModel?.Object.LoadCloud(value);
			OnPropertyChanged();
		}
	}

	public MainWindowViewModel(
		SimplePointCloudSceneViewModel simplePointCloudSceneViewModel,
		LoadPointCloudService loadPointCloudService)
	{
		this.loadPointCloudService = loadPointCloudService;

		SceneTypes.Add(new(simplePointCloudSceneViewModel, simplePointCloudSceneViewModel.Name));
		NamedSceneViewModel = SceneTypes[0];
	}

	[RelayCommand]
	public async Task OpenFileAsync()
	{
		ofd.Filter = ".xyz|*.xyz";
		ofd.Multiselect = false;

		if (ofd.ShowDialog() == true)
		{
			var cloud = await loadPointCloudService.LoadAsync(ofd.FileName);
			if (cloud is not null)
				Cloud = cloud;
		}
	}
}
