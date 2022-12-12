using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace PointCloudRenderer.APP.Extensions;

internal class SliderExtensions
{
	public static readonly DependencyProperty DragCompletedCommandProperty = 
		DependencyProperty.RegisterAttached("DragCompletedCommand", typeof(ICommand), typeof(SliderExtensions),
		new PropertyMetadata(default(ICommand), OnDragCompletedCommandChanged));

	public static readonly DependencyProperty DragCompletedCommandParameterProperty =
		DependencyProperty.RegisterAttached("DragCompletedCommandParameter", typeof(object), typeof(SliderExtensions),
		new PropertyMetadata(default, OnDragCompletedCommandChanged));

	public static readonly DependencyProperty DragStartedCommandProperty =
		DependencyProperty.RegisterAttached("DragStartedCommand", typeof(ICommand), typeof(SliderExtensions),
		new PropertyMetadata(default(ICommand), OnDragStartedCommandChanged));

	public static readonly DependencyProperty DragStartedCommandParameterProperty =
		DependencyProperty.RegisterAttached("DragStartedCommandParameter", typeof(object), typeof(SliderExtensions),
		new PropertyMetadata(default, OnDragStartedCommandChanged));

	private static void OnDragCompletedCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
	{
		var slider = dependencyObject as Slider;
		if (slider is null)
			return;

		if (e.NewValue is ICommand)
		{
			slider.Loaded += SliderOnDragCompletedLoaded;
		}
	}

	private static void OnDragStartedCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
	{
		var slider = dependencyObject as Slider;
		if (slider is null)
			return;

		if (e.NewValue is ICommand)
		{
			slider.Loaded += SliderOnDragStartedLoaded;
		}
	}

	private static void SliderOnDragCompletedLoaded(object sender, RoutedEventArgs e)
	{
		var slider = sender as Slider;
		if (slider is null)
			return;

		slider.Loaded -= SliderOnDragCompletedLoaded;

		var track = slider.Template.FindName("PART_Track", slider) as Track;
		if (track is null)
			return;

		track.Thumb.DragCompleted += (dragCompletedSender, dragCompletedArgs) =>
		{
			var command = GetDragCompletedCommand(slider);
			var param = GetDragCompletedCommandParamete(slider);
			command.Execute(param);
		};
	}

	private static void SliderOnDragStartedLoaded(object sender, RoutedEventArgs e)
	{
		var slider = sender as Slider;
		if (slider is null)
			return;

		slider.Loaded -= SliderOnDragStartedLoaded;

		var track = slider.Template.FindName("PART_Track", slider) as Track;
		if (track is null)
			return;

		track.Thumb.DragStarted += (dragStartedSender, dragStartedArgs) =>
		{
			var command = GetDragStartedCommand(slider);
			var param = GetDragStartedCommandParamete(slider);
			command.Execute(param);
		};
	}

	public static void SetDragCompletedCommand(DependencyObject element, ICommand value) =>
		element.SetValue(DragCompletedCommandProperty, value);

	public static ICommand GetDragCompletedCommand(DependencyObject element) =>
		(ICommand)element.GetValue(DragCompletedCommandProperty);

	public static void SetDragCompletedCommandParameter(DependencyObject element, object value) =>
		element.SetValue(DragCompletedCommandParameterProperty, value);

	public static object GetDragCompletedCommandParamete(DependencyObject element) =>
		element.GetValue(DragCompletedCommandParameterProperty);

	public static void SetDragStartedCommand(DependencyObject element, ICommand value) =>
		element.SetValue(DragStartedCommandProperty, value);

	public static ICommand GetDragStartedCommand(DependencyObject element) =>
		(ICommand)element.GetValue(DragStartedCommandProperty);

	public static void SetDragStartedCommandParameter(DependencyObject element, object value) =>
		element.SetValue(DragStartedCommandParameterProperty, value);

	public static object GetDragStartedCommandParamete(DependencyObject element) =>
		element.GetValue(DragStartedCommandParameterProperty);
}
