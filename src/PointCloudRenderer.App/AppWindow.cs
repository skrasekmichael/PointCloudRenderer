using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace PointCloudRenderer.App;

public class AppWindow : GameWindow
{
	public AppWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
	{
	}

	protected override void OnLoad()
	{
		base.OnLoad();
	}

	protected override void OnRenderFrame(FrameEventArgs args)
	{
		base.OnRenderFrame(args);
	}

	protected override void OnUnload()
	{
		base.OnUnload();
	}
}
