using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace PointCloudRenderer.APP.Services;

public sealed class OpenedRecentlyService
{
	public ObservableCollection<string> Files { get; } = new();

	private readonly string filePath;

	public OpenedRecentlyService()
	{
		var attr = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>();
		if (attr is null)
			throw new ArgumentNullException("Missing Guid Attribute.");

		var tmp = Path.GetTempPath();
		filePath = $"{tmp}/{attr.Value}_recently_open";
	}

	public async Task LoadAsync()
	{
		await Task.Run(() =>
		{
			if (File.Exists(filePath))
			{
				using var sr = new StreamReader(filePath);

				string? line;
				while ((line = sr.ReadLine()) is not null)
				{
					if (File.Exists(line) && !Files.Contains(line))
					{
						Files.Add(line);
						if (Files.Count == 10)
							break;
					}
				}
			}
		});
	}

	public async Task SaveAsync() => await File.WriteAllLinesAsync(filePath, Files);

	public async Task AddFileAsync(string path)
	{
		Files.Remove(path);
		Files.Insert(0, path);

		if (Files.Count > 10)
			Files.RemoveAt(Files.Count - 1);

		await SaveAsync();
	}

	public async Task ClearAsync()
	{
		if (Files.Count > 0)
		{
			Files.Clear();
			await SaveAsync();
		}
	}
}
