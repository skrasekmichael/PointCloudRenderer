using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.APP.ViewModels;
using System.Windows;

namespace PointCloudRenderer.APP.Views;
public partial class MainWindow : Window
{
	private readonly MainWindowViewModel viewModel;

	public MainWindow(MainWindowViewModel viewModel)
	{
		this.viewModel = viewModel;

		InitializeComponent();
		DataContext = viewModel;

		canvas.Start(new()
		{
			MajorVersion = 3,
			MinorVersion = 1,
			RenderContinuously = true
		});
	}

	private void canvas_Render(TimeSpan obj)
	{
		viewModel.Scene.Render();
	}

	private void canvas_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
	{
		viewModel.Scene.Zoom(e.Delta);
	}

	private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		viewModel.Scene.Width = (float)canvas.ActualWidth;
		viewModel.Scene.Height = (float)canvas.ActualHeight;
	}
}
