using PointCloudRenderer.APP.ViewModels;
using System.Windows;

namespace PointCloudRenderer.APP.Views;

public sealed partial class MainWindow : Window
{
	public MainWindow(MainWindowViewModel viewModel)
	{
		InitializeComponent();
		DataContext = viewModel;

		Canvas.Start(new()
		{
			MajorVersion = 4,
			MinorVersion = 6,
			RenderContinuously = true
		});

		Canvas.Render += _ => viewModel.NamedSceneViewModel?.Object.Scene.Render();
	}
}
