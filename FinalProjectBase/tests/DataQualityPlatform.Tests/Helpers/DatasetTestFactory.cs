using DataQualityPlatform.Models;

namespace DataQualityPlatform.Tests.Helpers;

public static class DatasetTestFactory
{
    public static Dataset CreateSingleRecord(
        params (string Column, string? Value)[] values)
    {
        Dictionary<string, string?> record = values.ToDictionary(
            value => value.Column,
            value => value.Value);

        return CreateRecords(record);
    }

    public static Dataset CreateRecords(
        params Dictionary<string, string?>[] records)
    {
        List<string> columnNames = records
            .SelectMany(record => record.Keys)
            .Distinct()
            .ToList();

        Dataset dataset = new()
        {
            Name = "Test Dataset",
            Columns = columnNames
                .Select(column => new ColumnDefinition
                {
                    Name = column,
                    DetectedType = DataType.String,
                    IsNullable = true
                })
                .ToList()
        };

        for (int index = 0; index < records.Length; index++)
        {
            dataset.Records.Add(new DataRecord
            {
                RowNumber = index + 1,
                Values = new Dictionary<string, string?>(records[index])
            });
        }

        return dataset;
    }

    public static Dataset CreateRectangularDataset(
        int rowCount,
        int columnCount)
    {
        Dataset dataset = new()
        {
            Name = "Rectangular Test Dataset"
        };

        for (int columnIndex = 1; columnIndex <= columnCount; columnIndex++)
        {
            dataset.Columns.Add(new ColumnDefinition
            {
                Name = $"Column{columnIndex}",
                DetectedType = DataType.String,
                IsNullable = true
            });
        }

        for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
        {
            Dictionary<string, string?> values = new();

            foreach (ColumnDefinition column in dataset.Columns)
            {
                values[column.Name] = $"R{rowIndex}{column.Name}";
            }

            dataset.Records.Add(new DataRecord
            {
                RowNumber = rowIndex,
                Values = values
            });
        }

        return dataset;
    }
}
