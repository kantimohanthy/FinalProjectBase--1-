using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

public class RemoveDuplicateTransformation : IDataTransformation
{
    public RemoveDuplicateTransformation(string columnName)
    {
        ColumnName = columnName;
    }

    public string Name => "Remove Duplicate Records";

    public string ColumnName { get; }

    public Dataset Apply(Dataset dataset)
    {
        HashSet<string?> encounteredValues = new();

        Dataset transformed = new()
        {
            Name = dataset.Name,

            Columns = dataset.Columns
                .Select(column => new ColumnDefinition
                {
                    Name = column.Name,
                    DetectedType = column.DetectedType,
                    IsNullable = column.IsNullable
                })
                .ToList()
        };

        foreach (DataRecord record in dataset.Records)
        {
            string? value = record.Values.GetValueOrDefault(ColumnName);

            if (!encounteredValues.Add(value))
            {
                continue;
            }

            transformed.Records.Add(new DataRecord
            {
                RowNumber = record.RowNumber,
                Values = new Dictionary<string, string?>(record.Values)
            });
        }

        return transformed;
    }
}