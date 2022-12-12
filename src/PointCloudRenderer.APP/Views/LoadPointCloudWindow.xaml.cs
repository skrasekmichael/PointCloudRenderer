using PointCloudRenderer.APP.ViewModels;
using System.Windows;

namespace PointCloudRenderer.APP.Views;

public sealed partial class LoadPointCloudWindow : Window
{
	public LoadPointCloudWindowViewModel ViewModel { get; }

	public LoadPointCloudWindow(LoadPointCloudWindowViewModel viewModel)
	{
		InitializeComponent();

		DataContext = viewModel;
		ViewModel = viewModel;
	}
}
