using PointCloudRenderer.Data.Enums;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PointCloudRenderer.Data.Parser;

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

	public string[] GetScalars(string line)
	{
		var match = regex.Match(line);

		if (!match.Success)
			throw new Exception();

		return match.Groups.Values.Select(x => x.Value).Skip(1).ToArray();
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

		var x = parseGroup(ScalarName.X.ToString());
		var y = parseGroup(ScalarName.Y.ToString());
		var z = parseGroup(ScalarName.Z.ToString());

		if (x is null || y is null || z is null)
			throw new Exception("Missing coordinates.");

		var r = parseGroup(ScalarName.R.ToString()) ?? 0.1f;
		var g = parseGroup(ScalarName.G.ToString()) ?? 0.0f;
		var b = parseGroup(ScalarName.B.ToString()) ?? 0.0f;
		var a = parseGroup(ScalarName.A.ToString()) ?? 1;

		return new()
		{
			Point = new(x.Value, y.Value, z.Value),
			Color = new(r, g, b, a)
		};
	}
}
