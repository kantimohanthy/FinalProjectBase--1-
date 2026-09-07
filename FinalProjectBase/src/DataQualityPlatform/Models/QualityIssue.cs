namespace DataQualityPlatform.Models;

public class QualityIssue
{
    public int RowNumber { get; set; }

    public string ColumnName { get; set; } = string.Empty;

    public string RuleName { get; set; } = string.Empty;

    public string? InvalidValue { get; set; }

    public string Message { get; set; } = string.Empty;

    public IssueSeverity Severity { get; set; }
}
