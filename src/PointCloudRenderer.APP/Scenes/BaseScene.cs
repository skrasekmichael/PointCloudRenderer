using CommunityToolkit.Mvvm.ComponentModel;
using PointCloudRenderer.APP.Helpers.Models;
using PointCloudRenderer.Data;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PointCloudRenderer.APP.Helpers;

namespace PointCloudRenderer.APP.Scenes;

public abstract partial class BaseScene : ObservableObject, IDisposable
{
	private const float FOVY = MathF.PI / 2;
	private static readonly float SCALE = MathF.Tan(MathF.PI / 4);

	protected AxisLinesModel? AxisLinesModel { get; private set; }
	protected AxisCirclesModel? AxisCirclesModel { get; private set; }

	[ObservableProperty]
	private float zoomLevel = 0.5f;

	[ObservableProperty]
	private float axisSize = 0.5f;

	[ObservableProperty]
	private bool displayAxis = false;

	private Quaternion currQuat = Quaternion.Identity, lastQuat = Quaternion.Identity;
	private Quaternion _quaternion = Quaternion.Identity;
	private Quaternion Quaternion
	{
		get => _quaternion;
		set
		{
			_quaternion = value;
			OnPropertyChanged(nameof(QuaternionX));
			OnPropertyChanged(nameof(QuaternionY));
			OnPropertyChanged(nameof(QuaternionZ));
			OnPropertyChanged(nameof(QuaternionW));
		}
	}

	public float QuaternionX
	{
		get => Quaternion.X;
		set => Quaternion = new Quaternion(value, QuaternionY, QuaternionZ, QuaternionW);
	}

	public float QuaternionY
	{
		get => Quaternion.Y;
		set => Quaternion = new Quaternion(QuaternionX, value, QuaternionZ, QuaternionW);
	}

	public float QuaternionZ
	{
		get => Quaternion.Z;
		set => Quaternion = new Quaternion(QuaternionX, QuaternionY, value, QuaternionW);
	}

	public float QuaternionW
	{
		get => Quaternion.W;
		set => Quaternion = new Quaternion(QuaternionX, QuaternionY, QuaternionZ, value);
	}

	private float _width = 1;
	public float Width
	{
		get => _width;
		set
		{
			_width = value;
			aspect = _width / _height;
		}
	}

	private float _height = 1;
	public float Height
	{
		get => _height;
		set
		{
			_height = value;
			aspect = _width / _height;
		}
	}

	private float aspect = 1.0f;

	public void Load(PointCloud cloud)
	{
		GL.Enable(EnableCap.DepthTest);
		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		AxisLinesModel?.Dispose();
		AxisLinesModel = new();

		AxisCirclesModel?.Dispose();
		AxisCirclesModel = new();

		LoadCloud(cloud);
	}

	protected abstract void LoadCloud(PointCloud cloud);

	public void Render()
	{
		GL.ClearColor(0.7f, 0.7f, 0.7f, 1);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		var proj = Matrix4.CreateTranslation(0, 0, -ZoomLevel) * Matrix4.CreatePerspectiveFieldOfView(FOVY, aspect, 0.001f, 150.0f);
		var model = Matrix4.CreateFromQuaternion(Quaternion);

		RenderCloud(model, proj);

		var mvpAxis = model * Matrix4.CreateScale(AxisSize * ZoomLevel * SCALE) * proj;

		if (DisplayAxis)
		{
			AxisLinesModel?.Render(mvpAxis);
			AxisCirclesModel?.Render(mvpAxis);
		}

		GL.Finish();
	}

	protected abstract void RenderCloud(Matrix4 model, Matrix4 proj);

	public virtual void Dispose()
	{
		AxisLinesModel?.Dispose();
		AxisCirclesModel?.Dispose();
	}

	public void StartRotaring()
	{
		lastQuat = Quaternion;
	}

	public void Rotate(double cx, double cy, double nx, double ny)
	{
		var a = Project(cx, cy);
		var b = Project(nx, ny);

		currQuat = QuaternionUtils.FromVectors(a, b);
		Quaternion = currQuat * lastQuat;
	}

	public void StopRotating()
	{
		lastQuat = Quaternion;
		currQuat = Quaternion.Identity;
	}

	private Vector3d Project(double x, double y)
	{
		//https://www.xarg.org/2021/07/trackball-rotation-using-quaternions/
		//http://orion.lcg.ufrj.br/games/ArcBall/Arcball.pdf

		var invRadius = 1 / Math.Min(Width, Height);

		x = (x - 0.5 * Width) * invRadius;
		y = (0.5 * Height - y) * invRadius;

		var x2 = x * x;
		var y2 = y * y;

		double z = 0;
		double r2 = x2 + y2;

		if (r2 > 1)
		{
			var s = 1 / Math.Sqrt(r2);
			x *= s;
			y *= s;
		}
		else
		{
			z = Math.Sqrt(1 - r2);
		}

		return new Vector3d(x, y, z);
	}
}
