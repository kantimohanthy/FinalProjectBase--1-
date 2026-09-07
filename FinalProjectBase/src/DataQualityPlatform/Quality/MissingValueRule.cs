using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class MissingValueRule : QualityRule
{
    public MissingValueRule(string columnName, bool isCritical = false)
        : base("Missing Value Rule", columnName, isCritical)
    {
    }

    public override List<QualityIssue> Evaluate(Dataset dataset)
    {
        // TODO(STUDENT): Throw ColumnNotFoundException when ColumnName is not present.
        // TODO(STUDENT): Detect null, empty, and whitespace-only values.
        // TODO(STUDENT): Create one QualityIssue per missing value.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
