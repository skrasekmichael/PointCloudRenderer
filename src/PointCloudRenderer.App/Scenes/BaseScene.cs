using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.Scenes;

public interface IScene
{
	public abstract void Load(PointCloud cloud);
	public abstract void Render();
}
