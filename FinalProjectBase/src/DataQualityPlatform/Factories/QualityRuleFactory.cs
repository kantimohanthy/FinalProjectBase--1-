using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;
using DataQualityPlatform.Quality;

namespace DataQualityPlatform.Factories;

public class QualityRuleFactory
{
    public IQualityRule Create(QualityRuleConfiguration configuration)
    {
        return configuration.Type switch
        {
            QualityRuleType.MissingValue =>
                new MissingValueRule(
                    configuration.ColumnName,
                    configuration.IsCritical),

            QualityRuleType.UniqueValue =>
                new UniqueValueRule(
                    configuration.ColumnName,
                    configuration.IsCritical),

            QualityRuleType.Range when
                configuration.Minimum.HasValue &&
                configuration.Maximum.HasValue =>
                new RangeRule(
                    configuration.ColumnName,
                    configuration.Minimum.Value,
                    configuration.Maximum.Value,
                    configuration.IsCritical),

            QualityRuleType.Range =>
                throw new ArgumentException(
                    "Range rules require Minimum and Maximum values."),

            QualityRuleType.Regex when
                !string.IsNullOrWhiteSpace(configuration.Pattern) =>
                new RegexRule(
                    configuration.ColumnName,
                    configuration.Pattern,
                    configuration.IsCritical),

            QualityRuleType.Regex =>
                throw new ArgumentException(
                    "Regex rules require a pattern."),

            QualityRuleType.DateFormat =>
                new DateFormatRule(
                    configuration.ColumnName,
                    configuration.IsCritical),

            _ => throw new UnsupportedRuleException(
                $"Unsupported quality rule type: {configuration.Type}")
        };
    }
}