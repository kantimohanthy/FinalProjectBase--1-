using DataQualityPlatform.Models;

namespace DataQualityPlatform.Services;

public class QualityScoreCalculator
{
    public double Calculate(
        Dataset dataset,
        IEnumerable<QualityIssue> issues)
    {
        if (dataset.Records.Count == 0 || dataset.Columns.Count == 0)
        {
            return 0;
        }

        long totalCells =
            (long)dataset.Records.Count * dataset.Columns.Count;

        int invalidCellCount = issues
            .Select(issue => (issue.RowNumber, issue.ColumnName))
            .Distinct()
            .Count();

        double score =
            100.0 - ((double)invalidCellCount / totalCells * 100.0);

        return Math.Clamp(score, 0.0, 100.0);
    }
}