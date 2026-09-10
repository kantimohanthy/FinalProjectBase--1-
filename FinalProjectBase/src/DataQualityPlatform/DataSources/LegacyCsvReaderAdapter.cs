using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;

namespace DataQualityPlatform.DataSources;

/// <summary>
/// Adapts LegacyCsvReader to the IDataSource interface.
/// </summary>
public class LegacyCsvReaderAdapter : IDataSource
{
    private readonly LegacyCsvReader _legacyReader;

    public LegacyCsvReaderAdapter(
        LegacyCsvReader legacyReader)
    {
        _legacyReader = legacyReader;
    }

    public Dataset Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new DatasetNotFoundException(
                $"Dataset file not found: {path}");
        }

        string[] lines = _legacyReader.ReadFile(path);

        if (lines.Length == 0)
        {
            throw new InvalidDatasetFormatException(
                "Dataset file is empty.");
        }

        string[] headers = lines[0].Split(',');

        bool hasAllRequiredColumns =
            CsvDataSource.RequiredColumns.All(
                requiredColumn =>
                    headers.Contains(requiredColumn));

        if (!hasAllRequiredColumns)
        {
            throw new InvalidDatasetFormatException(
                "One or more required columns are missing.");
        }

        Dataset dataset = new()
        {
            Name = Path.GetFileName(path)
        };

        foreach (string header in headers)
        {
            dataset.Columns.Add(
                new ColumnDefinition
                {
                    Name = header
                });
        }

        for (
            int lineIndex = 1;
            lineIndex < lines.Length;
            lineIndex++)
        {
            string[] values =
                lines[lineIndex].Split(',');

            if (values.Length != headers.Length)
            {
                throw new InvalidDatasetFormatException(
                    $"Row {lineIndex} does not match " +
                    "the header column count.");
            }

            DataRecord record = new()
            {
                RowNumber = lineIndex
            };

            for (
                int columnIndex = 0;
                columnIndex < headers.Length;
                columnIndex++)
            {
                record.Values[headers[columnIndex]] =
                    values[columnIndex];
            }

            dataset.Records.Add(record);
        }

        return dataset;
    }
}