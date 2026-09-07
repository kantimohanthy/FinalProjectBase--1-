using DataQualityPlatform.Models;

namespace DataQualityPlatform.Services;

public class QualityScoreCalculator
{
    public double Calculate(
        Dataset dataset,
        IEnumerable<QualityIssue> issues)
    {
        // TODO(STUDENT): Calculate total cells as row count multiplied by column count.
        // TODO(STUDENT): Count invalid cells as distinct RowNumber and ColumnName pairs.
        // TODO(STUDENT): Calculate score as 100 minus the invalid cell percentage.
        // TODO(STUDENT): Clamp the score between 0 and 100.
        // TODO(STUDENT): Return 0 for an empty dataset.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
