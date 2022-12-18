using OpenTK.Mathematics;

namespace PointCloudRenderer.APP.Helpers;

public static class QuaternionUtils
{
	public static Quaternion FromVectors(Vector3d v1, Vector3d v2)
	{
		//https://www.xarg.org/proof/quaternion-from-two-vectors/

		var d = Vector3d.Dot(v1, v2);
		var c = Vector3d.Cross(v1, v2);

		var v = d + Math.Sqrt(d * d + Vector3d.Dot(c, c));

		var quat = new Quaterniond(c, v);
		quat.Normalize();

		return new Quaternion((float)quat.X, (float)quat.Y, (float)quat.Z, (float)quat.W);
	}
}
