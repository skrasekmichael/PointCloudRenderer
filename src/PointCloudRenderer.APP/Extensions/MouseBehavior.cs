using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace PointCloudRenderer.APP.Extensions;

internal sealed class MouseBehavior : Behavior<UIElement>
{
	public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(nameof(MouseX), typeof(double), typeof(MouseBehavior), new(default(double)));
	public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(nameof(MouseY), typeof(double), typeof(MouseBehavior), new(default(double)));

	public double MouseX
	{
		get => (double)GetValue(MouseXProperty);
		set => SetValue(MouseXProperty, value);
	}

	public double MouseY
	{
		get => (double)GetValue(MouseYProperty);
		set => SetValue(MouseYProperty, value);
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
