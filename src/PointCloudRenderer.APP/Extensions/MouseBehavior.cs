using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace PointCloudRenderer.APP.Extensions;

internal sealed class MouseBehavior : Behavior<UIElement>
{
	public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
		"MouseY", typeof(double), typeof(MouseBehavior), new PropertyMetadata(default(double)));

	public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
		"MouseX", typeof(double), typeof(MouseBehavior), new PropertyMetadata(default(double)));

	public double MouseY
	{
		get => (double)GetValue(MouseYProperty);
		set => SetValue(MouseYProperty, value);
	}

	public double MouseX
	{
		get => (double)GetValue(MouseXProperty);
		set => SetValue(MouseXProperty, value);
	}

	protected override void OnAttached()
	{
		AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
	}

	private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
	{
		var pos = mouseEventArgs.GetPosition(AssociatedObject);
		MouseX = pos.X;
		MouseY = pos.Y;
	}

	protected override void OnDetaching()
	{
		AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
	}
}
