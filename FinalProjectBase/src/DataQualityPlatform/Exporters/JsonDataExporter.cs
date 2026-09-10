using System.Text.Json;
using DataQualityPlatform.Models;

namespace DataQualityPlatform.Exporters;

public class JsonDataExporter : IDataExporter
{
    public string FormatName => "JSON";

    public void Export(Dataset dataset, string path)
    {
        string? directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(dataset, options);
        File.WriteAllText(path, json);
    }
}