namespace DataQualityPlatform.Persistence.Entities;

public class ProcessingRunEntity
{
    public int Id { get; set; }

    public string DatasetName { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string Status { get; set; } = string.Empty;

    public int InputRecordCount { get; set; }

    public int OutputRecordCount { get; set; }

    public double QualityScore { get; set; }

    public List<QualityIssueEntity> QualityIssues { get; set; } = new();
}
