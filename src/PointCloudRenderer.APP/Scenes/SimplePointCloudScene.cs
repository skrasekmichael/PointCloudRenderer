using PointCloudRenderer.Data;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PointCloudRenderer.APP.Helpers.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PointCloudRenderer.APP.Scenes;

public sealed partial class SimplePointCloudScene : BaseScene
{
	private SimplePointCloudModel? pointCloudModel = null;

	protected override void LoadCloud(PointCloud cloud)
	{
		pointCloudModel = new SimplePointCloudModel(cloud);
	}

	protected override void RenderCloud(Matrix4 model, Matrix4 proj)
	{
		var mvpCloud = model * Matrix4.CreateTranslation(0, 0, -ZoomLevel) * proj;
		pointCloudModel?.Render(mvpCloud, proj);
	}

	public void SetPointSize(float size)
	{
		pointCloudModel?.SetPointSize(size);
	}

	public override void Dispose()
	{
		pointCloudModel?.Dispose();
		base.Dispose();
	}
}
