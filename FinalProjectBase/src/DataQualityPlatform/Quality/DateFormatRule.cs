using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class DateFormatRule : QualityRule
{
    public DateFormatRule(string columnName, bool isCritical = false)
        : base("Date Format Rule", columnName, isCritical)
    {
    }

    public string RequiredFormat => "yyyy-MM-dd";

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
                continue;
            }

            bool isValidDate = DateTime.TryParseExact(
                value,
                RequiredFormat,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out _);

            if (!isValidDate)
            {
                issues.Add(new QualityIssue
                {
                    RowNumber = record.RowNumber,
                    ColumnName = ColumnName,
                    RuleName = Name,
                    InvalidValue = value,
                    Message = $"Value is not in the required date format ({RequiredFormat}).",
                    Severity = IsCritical ? IssueSeverity.Error : IssueSeverity.Warning
                });
            }
        }

        return issues;
    }
}