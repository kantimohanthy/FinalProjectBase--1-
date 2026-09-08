using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class UniqueValueRule : QualityRule
{
    public UniqueValueRule(string columnName, bool isCritical = false)
        : base("Unique Value Rule", columnName, isCritical)
    {
    }

    public override List<QualityIssue> Evaluate(Dataset dataset)
    {
        if (!dataset.Columns.Any(column => column.Name == ColumnName))
        {
            throw new ColumnNotFoundException(ColumnName);
        }

        List<QualityIssue> issues = new();
        HashSet<string> seenValues = new();

        foreach (DataRecord record in dataset.Records)
        {
            string? value = GetValue(record);

            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            if (!seenValues.Add(value))
            {
                issues.Add(new QualityIssue
                {
                    RowNumber = record.RowNumber,
                    ColumnName = ColumnName,
                    RuleName = Name,
                    InvalidValue = value,
                    Message = "Duplicate value detected.",
                    Severity = IsCritical ? IssueSeverity.Error : IssueSeverity.Warning
                });
            }
        }

        return issues;
    }
}