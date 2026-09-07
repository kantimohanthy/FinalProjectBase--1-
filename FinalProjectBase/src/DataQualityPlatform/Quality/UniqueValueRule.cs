using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class UniqueValueRule : QualityRule
{
    public UniqueValueRule(string columnName, bool isCritical = false)
        : base("Unique Value Rule", columnName, isCritical)
    {
    }

    public override List<QualityIssue> Evaluate(Dataset dataset)
    {
        // TODO(STUDENT): Throw ColumnNotFoundException when ColumnName is not present.
        // TODO(STUDENT): Treat the first occurrence of each value as valid.
        // TODO(STUDENT): Generate an issue for each later duplicate occurrence.
        // TODO(STUDENT): Ignore missing values because MissingValueRule handles them.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
