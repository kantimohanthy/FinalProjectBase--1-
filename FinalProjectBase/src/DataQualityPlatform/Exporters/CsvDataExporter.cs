using DataQualityPlatform.Models;

namespace DataQualityPlatform.Exporters;

public class CsvDataExporter : IDataExporter
{
    public string FormatName => "CSV";

    public void Export(Dataset dataset, string path)
    {
        string? directory = Path.GetDirectoryName(path);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using StreamWriter writer = new(path);

        writer.WriteLine(string.Join(
            ",",
            dataset.Columns.Select(column => column.Name)));

        foreach (DataRecord record in dataset.Records)
        {
            IEnumerable<string?> values = dataset.Columns.Select(
                column => record.Values.GetValueOrDefault(column.Name));

            writer.WriteLine(string.Join(",", values));
        }
    }
}