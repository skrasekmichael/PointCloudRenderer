using PointCloudRenderer.APP.ViewModels;
using System.Windows;

namespace PointCloudRenderer.APP.Views;

public sealed partial class MainWindow : Window
{
	public MainWindow(MainWindowViewModel viewModel)
	{
		InitializeComponent();
		DataContext = viewModel;

		canvas.Start(new()
		{
			MajorVersion = 3,
			MinorVersion = 1,
			RenderContinuously = true,
		});

		canvas.Render += _ => viewModel.NamedSceneViewModel?.Object.Scene.Render();
	}
}
