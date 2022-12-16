using PointCloudRenderer.Data;
using OpenTK.Mathematics;
using PointCloudRenderer.APP.Helpers.Models;

namespace PointCloudRenderer.APP.Scenes;

public sealed partial class SimplePointCloudScene : BaseScene
{
	private SimplePointCloudModel? pointCloudModel = null;

	protected override void LoadCloud(PointCloud cloud)
	{
		pointCloudModel?.Dispose();
		pointCloudModel = new SimplePointCloudModel(cloud);
	}

	protected override void RenderCloud(Matrix4 model, Matrix4 proj)
	{
		var mvpCloud = model * proj;
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
