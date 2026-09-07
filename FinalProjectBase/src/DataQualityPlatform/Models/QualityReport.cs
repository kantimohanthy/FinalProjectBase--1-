namespace DataQualityPlatform.Models;

public class QualityReport
{
    public string DatasetName { get; set; } = string.Empty;

    public DateTime ExecutionDate { get; set; }

    public int InitialRecordCount { get; set; }

    public int FinalRecordCount { get; set; }

    public double InitialScore { get; set; }

    public double FinalScore { get; set; }

    public List<QualityIssue> DetectedIssues { get; set; } = new();

    public List<string> AppliedTransformations { get; set; } = new();
}
