using OpenTK.Mathematics;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.APP.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace PointCloudRenderer.APP.Views;
public partial class MainWindow : Window
{
	private readonly MainWindowViewModel viewModel;
	private Point source;
	private bool isRotating = false;

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

	private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		viewModel.Zoom(e.Delta);
	}

	private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		viewModel.Scene.Width = (float)canvas.ActualWidth;
		viewModel.Scene.Height = (float)canvas.ActualHeight;
	}

	private void canvas_MouseMove(object sender, MouseEventArgs e)
	{
		
		if (isRotating)
		{
			var newSource = e.GetPosition(canvas);
			var delta = newSource - source;
			source = newSource;

			viewModel.OrbitAngleX = (viewModel.OrbitAngleX + (float)(delta.X * 0.01)) % MathF.Tau;
			viewModel.OrbitAngleY = (viewModel.OrbitAngleY + (float)(delta.Y * 0.01)) % MathF.Tau;
		}
	}

	private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		source = e.GetPosition(canvas);
		isRotating = true;
		viewModel.Scene.DisplayAxis = true;
		viewModel.Scene.DisplayCircleAxis = true;
	}

	private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		isRotating = false;
		viewModel.Scene.DisplayAxis = false;
		viewModel.Scene.DisplayCircleAxis = false;
	}

	private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
	{
		viewModel.Scene.DisplayAxis = false;
		viewModel.Scene.DisplayCircleAxis = false;
	}

	private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
	{
		viewModel.Scene.DisplayAxis = true;
		viewModel.Scene.DisplayCircleAxis = true;
	}
}
