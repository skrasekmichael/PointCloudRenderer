namespace PointCloudRenderer.APP.Helpers;

public sealed record NamedObject<T>
{
	public string Name { get; set; }
	public T Object { get; set; }

	public NamedObject(T @object, string name)
	{
		Name = name;
		Object = @object;
	}

	public override string ToString() => Name;
}
