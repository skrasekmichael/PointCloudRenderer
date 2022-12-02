using OpenTK.Mathematics;

namespace PointCloudRenderer.APP.Helpers.Models;

public class AxisLinesModel
{
	private readonly LineModel x, y, z;

	public AxisLinesModel()
	{
		x = new(new(0, 0, 0), new(1, 0, 0))
		{
			Color = new(1, 0, 0, 1)
		};

		y = new(new(0, 0, 0), new(0, 1, 0))
		{
			Color = new(0, 1, 0, 1)
		};

		z = new(new(0, 0, 0), new(0, 0, 1))
		{
			Color = new(0, 0, 1, 1)
		};
	}

	public void Render(Matrix4 mvp)
	{
		x.Render(mvp);
		y.Render(mvp);
		z.Render(mvp);
	}
}
