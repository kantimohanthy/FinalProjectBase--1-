using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;
using System.Globalization;

namespace DataQualityPlatform.Quality;

public class RangeRule : QualityRule
{
    public RangeRule(
        string columnName,
        decimal minimum,
        decimal maximum,
        bool isCritical = false)
        : base("Range Rule", columnName, isCritical)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public decimal Minimum { get; }

    public decimal Maximum { get; }

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

            bool isValidNumber = decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal parsedValue);

            if (!isValidNumber)
            {
                issues.Add(new QualityIssue
                {
                    RowNumber = record.RowNumber,
                    ColumnName = ColumnName,
                    RuleName = Name,
                    InvalidValue = value,
                    Message = "Value is not a valid number.",
                    Severity = IsCritical ? IssueSeverity.Error : IssueSeverity.Warning
                });

                continue;
            }

            if (parsedValue < Minimum || parsedValue > Maximum)
            {
                issues.Add(new QualityIssue
                {
                    RowNumber = record.RowNumber,
                    ColumnName = ColumnName,
                    RuleName = Name,
                    InvalidValue = value,
                    Message = $"Value must be between {Minimum} and {Maximum}.",
                    Severity = IsCritical ? IssueSeverity.Error : IssueSeverity.Warning
                });
            }
        }

        return issues;
    }
}