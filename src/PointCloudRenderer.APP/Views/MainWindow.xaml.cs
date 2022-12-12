using PointCloudRenderer.APP.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace PointCloudRenderer.APP.Views;

public sealed partial class MainWindow : Window
{
	public MainWindowViewModel ViewModel { get; }

	private Point source;
	private bool isRotating = false;

	public MainWindow(MainWindowViewModel viewModel)
	{
		InitializeComponent();

		ViewModel = viewModel;
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
		ViewModel.SceneViewModel.Scene.Render();
	}

	private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		ViewModel.SceneViewModel.Zoom(e.Delta);
	}

	private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		ViewModel.SceneViewModel.Scene.Width = (float)canvas.ActualWidth;
		ViewModel.SceneViewModel.Scene.Height = (float)canvas.ActualHeight;
	}

	private void canvas_MouseMove(object sender, MouseEventArgs e)
	{
		
		if (isRotating)
		{
			var newSource = e.GetPosition(canvas);
			var delta = newSource - source;
			source = newSource;

			ViewModel.SceneViewModel.Scene.OrbitAngleX = (ViewModel.SceneViewModel.Scene.OrbitAngleX + (float)(delta.X * 0.01)) % MathF.Tau + MathF.Tau;
			ViewModel.SceneViewModel.Scene.OrbitAngleY = (ViewModel.SceneViewModel.Scene.OrbitAngleY + (float)(delta.Y * 0.01)) % MathF.Tau + MathF.Tau;
		}
	}

	private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		source = e.GetPosition(canvas);
		isRotating = true;
		ViewModel.SceneViewModel.Scene.DisplayAxis = true;
		ViewModel.SceneViewModel.Scene.DisplayCircleAxis = true;
	}

	private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		isRotating = false;
		ViewModel.SceneViewModel.Scene.DisplayAxis = false;
		ViewModel.SceneViewModel.Scene.DisplayCircleAxis = false;
	}
}
