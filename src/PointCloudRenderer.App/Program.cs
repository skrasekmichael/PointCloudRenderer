using OpenTK.Windowing.Desktop;
using PointCloudRenderer.App;

var gameWindowSettings = new GameWindowSettings()
{
	RenderFrequency = 60
};

var nativeWindowSettings = new NativeWindowSettings()
{
	Title = "PointCloudRenderer",
	Size = new(800, 600)
};

var window = new AppWindow(gameWindowSettings, nativeWindowSettings);

window.Run();

window.Close();

