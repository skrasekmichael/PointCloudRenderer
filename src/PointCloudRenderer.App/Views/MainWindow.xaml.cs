﻿using OpenTK.Mathematics;
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

	private void canvas_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
	{
		viewModel.Zoom(e.Delta);
	}

	private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		viewModel.Scene.Width = (float)canvas.ActualWidth;
		viewModel.Scene.Height = (float)canvas.ActualHeight;
	}

	private void canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
	{
		
		if (isRotating)
		{
			var newSource = e.GetPosition(canvas);
			var delta = newSource - source;
			source = newSource;

			viewModel.OrbitAngleX = (viewModel.OrbitAngleX + (float)(delta.X)) % 360;
			viewModel.OrbitAngleY = (viewModel.OrbitAngleY + (float)(delta.Y)) % 360;
		}
	}

	private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		source = e.GetPosition(canvas);
		isRotating = true;
	}

	private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		isRotating = false;
	}
}
