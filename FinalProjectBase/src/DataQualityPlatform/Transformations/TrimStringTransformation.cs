using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

public class TrimStringTransformation : IDataTransformation
{
    public string Name => "Trim String Values";

    public Dataset Apply(Dataset dataset)
    {
        return new Dataset
        {
            Name = dataset.Name,

            Columns = dataset.Columns
                .Select(column => new ColumnDefinition
                {
                    Name = column.Name,
                    DetectedType = column.DetectedType,
                    IsNullable = column.IsNullable
                })
                .ToList(),

            Records = dataset.Records
                .Select(record => new DataRecord
                {
                    RowNumber = record.RowNumber,
                    Values = record.Values.ToDictionary(
                        pair => pair.Key,
                        pair => pair.Value?.Trim())
                })
                .ToList()
        };
    }
}