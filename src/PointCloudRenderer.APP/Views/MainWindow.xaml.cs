using PointCloudRenderer.APP.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace PointCloudRenderer.APP.Views;
public partial class MainWindow : Window
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
		ViewModel.Scene.Render();
	}

	private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		ViewModel.Zoom(e.Delta);
	}

	private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		ViewModel.Scene.Width = (float)canvas.ActualWidth;
		ViewModel.Scene.Height = (float)canvas.ActualHeight;
	}

	private void canvas_MouseMove(object sender, MouseEventArgs e)
	{
		
		if (isRotating)
		{
			var newSource = e.GetPosition(canvas);
			var delta = newSource - source;
			source = newSource;

			ViewModel.Scene.OrbitAngleX = MathF.Abs((ViewModel.Scene.OrbitAngleX + (float)(delta.X * 0.01)) % MathF.Tau);
			ViewModel.Scene.OrbitAngleY = MathF.Abs((ViewModel.Scene.OrbitAngleY + (float)(delta.Y * 0.01)) % MathF.Tau);
		}
	}

	private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		source = e.GetPosition(canvas);
		isRotating = true;
		ViewModel.Scene.DisplayAxis = true;
		ViewModel.Scene.DisplayCircleAxis = true;
	}

	private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		isRotating = false;
		ViewModel.Scene.DisplayAxis = false;
		ViewModel.Scene.DisplayCircleAxis = false;
	}

	private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
	{
		ViewModel.Scene.DisplayAxis = false;
		ViewModel.Scene.DisplayCircleAxis = false;
	}

	private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
	{
		ViewModel.Scene.DisplayAxis = true;
		ViewModel.Scene.DisplayCircleAxis = true;
	}
}
