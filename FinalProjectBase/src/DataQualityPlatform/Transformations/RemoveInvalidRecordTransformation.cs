using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

public class RemoveInvalidRecordTransformation : IDataTransformation
{
    private readonly List<QualityIssue> _issues;

    public RemoveInvalidRecordTransformation(IEnumerable<QualityIssue> issues)
    {
        _issues = issues.ToList();
    }

    public string Name => "Remove Invalid Records";

    public Dataset Apply(Dataset dataset)
    {
        HashSet<int> invalidRows = _issues
            .Where(issue => issue.Severity == IssueSeverity.Error)
            .Select(issue => issue.RowNumber)
            .ToHashSet();

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
                .Where(record => !invalidRows.Contains(record.RowNumber))
                .Select(record => new DataRecord
                {
                    RowNumber = record.RowNumber,
                    Values = new Dictionary<string, string?>(record.Values)
                })
                .ToList()
        };
    }
}