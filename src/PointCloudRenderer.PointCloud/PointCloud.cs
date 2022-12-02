using OpenTK.Mathematics;
using PointCloudRenderer.Data.Extensions;

namespace PointCloudRenderer.Data;

public class PointCloud
{
	public float[] Points { get; }
	public Vector3 Center { get; init; }

	public PointCloud(float[] data)
	{
		Points = data;
	}

	public static PointCloud FromFile(string path, LineParser parser)
	{
		var points = new List<float>();

		using var stream = File.OpenRead(path);
		using var reader = new StreamReader(stream);

		string? line = null;
		Vector3 sum = Vector3.Zero;
		int count = 0;

		do
		{
			var data = parser.Parse(line);
			if (data is not null)
			{
				points.Add(data.Point);
				points.Add(data.Color);

				sum += data.Point;
				count++;
			}
		} while ((line = reader.ReadLine()) is not null);

		return new PointCloud(points.ToArray())
		{
			Center = sum / count
		};
	}
}
