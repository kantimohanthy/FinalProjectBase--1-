using System.Text.RegularExpressions;
using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class RegexRule : QualityRule
{
    public RegexRule(
        string columnName,
        string pattern,
        bool isCritical = false)
        : base("Regex Rule", columnName, isCritical)
    {
        Pattern = pattern;
    }

    public string Pattern { get; }

    public override List<QualityIssue> Evaluate(Dataset dataset)
    {
        if (!dataset.Columns.Any(column => column.Name == ColumnName))
        {
            throw new ColumnNotFoundException(ColumnName);
        }

        List<QualityIssue> issues = new();
        Regex regex = new(Pattern);

        foreach (DataRecord record in dataset.Records)
        {
            string? value = GetValue(record);

            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            if (!regex.IsMatch(value))
            {
                issues.Add(new QualityIssue
                {
                    RowNumber = record.RowNumber,
                    ColumnName = ColumnName,
                    RuleName = Name,
                    InvalidValue = value,
                    Message = "Value does not match the required format.",
                    Severity = IsCritical ? IssueSeverity.Error : IssueSeverity.Warning
                });
            }
        }

        return issues;
    }
}