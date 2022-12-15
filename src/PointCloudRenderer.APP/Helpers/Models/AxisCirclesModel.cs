using OpenTK.Mathematics;

namespace PointCloudRenderer.APP.Helpers.Models;

public sealed class AxisCirclesModel : IModel
{
	private readonly CircleModel cx, cy, cz;

	public AxisCirclesModel()
	{

		cx = new(
			new(0, -1, 1),
			new(0, 1, 1),
			new(0, 1, -1),
			new(0, -1, -1)
		)
		{
			Color = new(1, 0, 0, 1)
		};

		cy = new(
			new(-1, 0, 1),
			new(1, 0, 1),
			new(1, 0, -1),
			new(-1, 0, -1)
		)
		{
			Color = new(0, 1, 0, 1)
		};

		cz = new(
			new(-1, 1, 0),
			new(1, 1, 0),
			new(1, -1, 0),
			new(-1, -1, 0)
		)
		{
			Color = new(0, 0, 1, 1)
		};
	}

	public void Dispose()
	{
		cx.Dispose();
		cy.Dispose();
		cz.Dispose();
	}

	public void Render(Matrix4 mvp)
	{
		cx.Render(mvp);
		cy.Render(mvp);
		cz.Render(mvp);
	}
}
