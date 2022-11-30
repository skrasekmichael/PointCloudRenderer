using System.Text;
using System.Text.RegularExpressions;
using PointCloudRenderer.Data.Enums;

namespace PointCloudRenderer.Data;

public sealed class LineParserBuilder
{
	private Dictionary<string, Func<string, IFormatProvider?, float?>> converters = new();
	private readonly BuilderOptions options;
	private readonly StringBuilder builder = new();
	private readonly string separator;

	private char firstSpace = '*';

	public LineParserBuilder(BuilderOptions options)
	{
		this.options = options;
		
		var separator2char = new string[4] { "\\s", "\\t", ",", ";" };
		separator = separator2char[(int)options.Separator];
	}

	public void AddPoint<T>(CoordinatesOrder order = CoordinatesOrder.XYZ) where T : IValueConverter
	{
		var axis = order.ToString();
		builder.Append($"""{separator}{firstSpace}(?<{axis[0]}>\-?\d+(?:\.\d+)?)""");
		builder.Append($"""{separator}+(?<{axis[1]}>\-?\d+(?:\.\d+)?)""");
		builder.Append($"""{separator}+(?<{axis[2]}>\-?\d+(?:\.\d+)?)""");

		converters.Add("X", T.Convert);
		converters.Add("Y", T.Convert);
		converters.Add("Z", T.Convert);

		firstSpace = '+';
	}

	public void AddColor<T>() where T : IValueConverter
	{
		builder.Append($"""{separator}{firstSpace}(?<R>\d+(?:\.\d+)?)""");
		builder.Append($"""{separator}+(?<G>\d+(?:\.\d+)?)""");
		builder.Append($"""{separator}+(?<B>\d+(?:\.\d+)?)""");

		converters.Add("R", T.Convert);
		converters.Add("G", T.Convert);
		converters.Add("B", T.Convert);

		firstSpace = '+';
	}

	public void AddIntensity<T>() where T : IValueConverter
	{
		builder.Append($"""{separator}{firstSpace}(?<A>\d+(?:\.\d +)?)""");
		converters.Add("A", T.Convert);

		firstSpace = '+';
	}

	public LineParser Build()
	{
		builder.Append($".*(?=(?:{string.Join('|', options.Comments)}|$))");
		var regex = new Regex(builder.ToString());
		
		var parser = new LineParser(converters, regex);

		builder.Clear();
		converters = new();
		firstSpace = '*';

		return parser;
	}
}
