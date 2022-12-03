namespace PointCloudRenderer.APP.Helpers;

public record NamedType
{
	public Type Type { get; set; }
	public string Name { get; set; }

	public NamedType(Type type, string name)
	{
		Type = type;
		Name = name;
	}

	public override string ToString() => Name;
}
