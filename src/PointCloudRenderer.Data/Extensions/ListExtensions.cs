using OpenTK.Mathematics;

namespace PointCloudRenderer.Data.Extensions;

public static class ListExtensions
{
	public static void Add(this List<float> list, Vector4 vector)
	{
		list.Add(vector.X);
		list.Add(vector.Y);
		list.Add(vector.Z);
		list.Add(vector.W);
	}

	public static void Add(this List<float> list, Vector3 vector)
	{
		list.Add(vector.X);
		list.Add(vector.Y);
		list.Add(vector.Z);
	}
}
