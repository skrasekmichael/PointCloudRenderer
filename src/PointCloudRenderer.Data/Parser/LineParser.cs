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
			return Array.Empty<string>();

		return match.Groups.Values.Select(x => x.Value).Skip(1).ToArray();
	}

	public LineData? Parse(string? line)
	{
		if (line is null)
			return null;

		var match = regex.Match(line);
		if (!match.Success)
			return null;

		var missingGroups = new List<string>();

		float? parseGroup(string group)
		{
			if (match.Groups.ContainsKey(group) && converters.ContainsKey(group))
			{
				var res = converters[group](match.Groups[group].Value, nfi);
				if (res is null)
					throw new Exception($"Failed to parse [{group}] at line [{line}]");

				return res;
			}

			missingGroups.Add(group);
			return null;
		}

		var x = parseGroup(ScalarName.X.ToString()) ?? 0.0f;
		var y = parseGroup(ScalarName.Y.ToString()) ?? 0.0f;
		var z = parseGroup(ScalarName.Z.ToString()) ?? 0.0f;
		var r = parseGroup(ScalarName.R.ToString()) ?? 0.1f;
		var g = parseGroup(ScalarName.G.ToString()) ?? 0.0f;
		var b = parseGroup(ScalarName.B.ToString()) ?? 0.0f;
		var a = parseGroup(ScalarName.A.ToString()) ?? 1;

		return new()
		{
			Point = new(x, y, z),
			Color = new(r, g, b, a),
			Missing = missingGroups
		};
	}
}
