using PointCloudRenderer.Data.Extensions;

namespace PointCloudRenderer.Data;

public class PointCloud
{
	public float[] Points { get; }

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
		do
		{
			var data = parser.Parse(line);
			if (data is not null)
			{
				points.Add(data.Point);
				points.Add(data.Color);
			}
		} while ((line = reader.ReadLine()) is not null);

		return new PointCloud(points.ToArray());
	}
}
