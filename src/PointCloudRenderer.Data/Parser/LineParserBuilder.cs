using System.Text;
using System.Text.RegularExpressions;
using PointCloudRenderer.Data.Converters;
using PointCloudRenderer.Data.Enums;
using PointCloudRenderer.Data.Extensions;

namespace PointCloudRenderer.Data.Parser;

public sealed class LineParserBuilder
{
	private Dictionary<string, Func<string, IFormatProvider?, float?>> converters = new();
	private readonly LineFormatOptions options;
	private readonly StringBuilder builder = new("^ *");
	private readonly string separator;

	private string firstSpace = "{0}";

	public LineParserBuilder(LineFormatOptions options)
	{
		this.options = options;
		separator = options.Separator.ToStringSeparator();
	}

	public void AddScalar<T>(ScalarName name = ScalarName.None, bool signed = true) where T : IScalarConverter
	{
		var sign = signed ? "\\-?" : "";
		var groupName = name switch
		{
			ScalarName.None => "?:", //non capturing group
			ScalarName.Other => "", //not named group
			_ => $"?<{name}>"
		};

		builder.Append($"""{separator}{firstSpace} *({groupName}{sign}\d+(?:\.\d+)?) *""");

		if (name != ScalarName.None && name != ScalarName.Other)
			converters.Add(name.ToString(), T.Convert);

		firstSpace = "{1}";
	}

	public LineParser Build()
	{
		builder.Append($".*(?=(?:{string.Join('|', options.Comments)}|$))");
		var regex = new Regex(builder.ToString());

		var parser = new LineParser(converters, regex);

		builder.Clear();
		builder.Append("^ *");

		converters = new();
		firstSpace = "{0}";

		return parser;
	}
}
