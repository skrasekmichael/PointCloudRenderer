using System.Globalization;
using System.Text.RegularExpressions;

namespace PointCloudRenderer.Data;

public sealed class LineParser
{
	private static readonly NumberFormatInfo nfi = new()
	{
		NumberDecimalSeparator = ".",
	};

	private readonly Dictionary<string, Func<string, IFormatProvider?, float?>> converters;
	private readonly Regex regex;

	public LineParser(Dictionary<string, Func<string, IFormatProvider?, float?>> converters, Regex regex)
	{
		this.converters = converters;
		this.regex = regex;
	}

	public LineData? Parse(string? line)
	{
		if (line is null)
			return null;

		var match = regex.Match(line);
		if (!match.Success)
			return null;

		float? parseGroup(string group)
		{
			if (match.Groups.ContainsKey(group) && converters.ContainsKey(group))
				return converters[group](match.Groups[group].Value, nfi);
			return null;
		}

		var x = parseGroup("X");
		var y = parseGroup("Y");
		var z = parseGroup("Z");

		if (x is null || y is null || z is null)
			return null;

		var r = parseGroup("R") ?? 0.9f;
		var g = parseGroup("G") ?? 0.0f;
		var b = parseGroup("B") ?? 0.0f;
		var intensity = parseGroup("A") ?? 1;

		return new()
		{
			Point = new(x.Value, y.Value, z.Value),
			Color = new(r, g, b, intensity)
		};
	}
}
