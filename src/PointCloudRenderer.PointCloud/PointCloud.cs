using OpenTK.Mathematics;
using PointCloudRenderer.Data.Parser;

namespace PointCloudRenderer.Data;

public class PointCloud
{
	public float[] Points { get; }
	public Vector3 Center { get; init; }

	public PointCloud(float[] data)
	{
		Points = data;
	}
}
