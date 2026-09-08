using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class MissingValueRule : QualityRule
{
    public MissingValueRule(string columnName, bool isCritical = false)
        : base("Missing Value Rule", columnName, isCritical)
    {
    }

    public override List<QualityIssue> Evaluate(Dataset dataset)
    {
        if (!dataset.Columns.Any(column => column.Name == ColumnName))
        {
            throw new ColumnNotFoundException(ColumnName);
        }

        List<QualityIssue> issues = new();

        foreach (DataRecord record in dataset.Records)
        {
            string? value = GetValue(record);

            if (string.IsNullOrWhiteSpace(value))
            {
                issues.Add(new QualityIssue
                {
                    RowNumber = record.RowNumber,
                    ColumnName = ColumnName,
                    RuleName = Name,
                    InvalidValue = value,
                    Message = "Required value is missing.",
                    Severity = IsCritical ? IssueSeverity.Error : IssueSeverity.Warning
                });
            }
        }

        return issues;
    }
}