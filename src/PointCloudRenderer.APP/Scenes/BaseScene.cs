using CommunityToolkit.Mvvm.ComponentModel;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.Scenes;

public abstract class BaseScene : ObservableObject, IDisposable
{
	public abstract void Load(PointCloud cloud);
	public abstract void Render();
	public abstract void Dispose();
}
