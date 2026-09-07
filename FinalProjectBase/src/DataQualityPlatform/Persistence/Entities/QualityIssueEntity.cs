using DataQualityPlatform.Models;

namespace DataQualityPlatform.Persistence.Entities;

public class QualityIssueEntity
{
    public int Id { get; set; }

    public int ProcessingRunEntityId { get; set; }

    public int RowNumber { get; set; }

    public string ColumnName { get; set; } = string.Empty;

    public string RuleName { get; set; } = string.Empty;

    public string? InvalidValue { get; set; }

    public string Message { get; set; } = string.Empty;

    public IssueSeverity Severity { get; set; }

    public ProcessingRunEntity? ProcessingRun { get; set; }
}
