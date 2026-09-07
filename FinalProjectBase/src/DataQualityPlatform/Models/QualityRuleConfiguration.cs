namespace DataQualityPlatform.Models;

public class QualityRuleConfiguration
{
    public QualityRuleType Type { get; set; }

    public string ColumnName { get; set; } = string.Empty;

    public bool IsCritical { get; set; }

    public decimal? Minimum { get; set; }

    public decimal? Maximum { get; set; }

    public string? Pattern { get; set; }
}
