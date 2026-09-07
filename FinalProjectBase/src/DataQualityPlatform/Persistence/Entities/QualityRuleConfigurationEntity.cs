using DataQualityPlatform.Models;

namespace DataQualityPlatform.Persistence.Entities;

public class QualityRuleConfigurationEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public QualityRuleType RuleType { get; set; }

    public string ColumnName { get; set; } = string.Empty;

    public bool IsCritical { get; set; }

    public decimal? Minimum { get; set; }

    public decimal? Maximum { get; set; }

    public string? Pattern { get; set; }
}
