using DataQualityPlatform.Models;

namespace DataQualityPlatform.Services;

public class TransformationHistory
{
    private readonly Stack<Dataset> _history = new();

    public bool CanUndo => _history.Count > 0;

    public void Save(Dataset dataset)
    {
        _history.Push(CloneDataset(dataset));
    }

    public Dataset Undo()
    {
        if (!CanUndo)
        {
            throw new InvalidOperationException(
                "There is no transformation to undo.");
        }

        return _history.Pop();
    }

    private static Dataset CloneDataset(Dataset dataset)
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
                    Values = new Dictionary<string, string?>(record.Values)
                })
                .ToList()
        };
    }
}