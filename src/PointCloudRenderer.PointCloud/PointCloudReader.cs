using OpenTK.Mathematics;
using PointCloudRenderer.Data.Converters;
using PointCloudRenderer.Data.Enums;
using PointCloudRenderer.Data.Extensions;
using PointCloudRenderer.Data.Parser;
using System.Text.RegularExpressions;

namespace PointCloudRenderer.Data;

public sealed class PointCloudReader
{
	private readonly string[] lines;

	public PointCloudReader(string path)
	{
		lines = File.ReadAllLines(path);
	}

	public IEnumerable<int> GetNumberOfScalars(Range range, LineFormatOptions options)
	{
		var lines = this.lines[range];
		var separator = options.Separator.ToStringSeparator();

		var removeCommentsRegex = new Regex($"^(.*)(?=(?:{string.Join('|', options.Comments)}|$))");
		var countRegex = new Regex($"(?: *(?:\\-?\\d+(?:\\.\\d+)) *{separator}| *(?:\\-?\\d+(?:\\.\\d+)) *?)");

		foreach (var line in lines)
		{
			var match = removeCommentsRegex.Match(line);
			if (match.Success)
			{
				var data = match.Groups[0].Value.Trim();
				var countMatches = countRegex.Matches(data);
				yield return countMatches.Count;
			}
		}
	}

	public List<(int LineNum, string Scalar)[]> GetScalars(int numOfScalars, Range range, LineFormatOptions options)
	{
		var builder = new LineParserBuilder(options);

		for (int i = 0; i < numOfScalars; i++)
			builder.AddScalar<FloatScalar>(ScalarName.Other);

		var parser = builder.Build();
		var lines = this.lines[range];

		var scalars = new List<(int, string)[]>(numOfScalars);

		for (int i = 0; i < numOfScalars; i++)
		{
			scalars.Add(new (int, string)[lines.Length]);
		}

		for (int i = 0; i < lines.Length; i++)
		{
			var vals = parser.GetScalars(lines[i]);
			for (int j = 0; j < vals.Length; j++)
				scalars[j][i] = new(i + range.Start.Value, vals[j]);
		}

		return scalars;
	}

	public string[] GetLines(Range range) => lines[range];

	public PointCloud GetPointCloud(LineParser parser) => ParsePointCloud(parser).Cloud;
	public (PointCloud Cloud, IEnumerable<string> Missing) ParsePointCloud(LineParser parser)
	{
		var points = new List<float>();

		var sum = Vector3.Zero;
		var count = 0;

		var errors = new HashSet<string>();

		foreach (var line in lines)
		{
			var data = parser.Parse(line);
			if (data is not null)
			{
				points.Add(data.Point);
				points.Add(data.Color);
				foreach (var err in data.Missing)
					errors.Add(err);

				sum += data.Point;
				count++;
			}
		}

		return (new PointCloud(points.ToArray())
		{
			Center = sum / count,
			Count = count
		}, errors);
	}
}
