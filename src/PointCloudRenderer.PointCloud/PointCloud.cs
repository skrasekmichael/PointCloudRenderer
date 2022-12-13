using OpenTK.Mathematics;

namespace PointCloudRenderer.Data;

public class PointCloud
{
	public int Count { get; set; }
	public float[] Points { get; }
	public Vector3 Center { get; init; }

	public PointCloud(float[] data)
	{
		Points = data;
	}
}
