using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class RegexRule : QualityRule
{
    public RegexRule(
        string columnName,
        string pattern,
        bool isCritical = false)
        : base("Regex Rule", columnName, isCritical)
    {
        Pattern = pattern;
    }

    public string Pattern { get; }

    public override List<QualityIssue> Evaluate(Dataset dataset)
    {
        // TODO(STUDENT): Throw ColumnNotFoundException when ColumnName is not present.
        // TODO(STUDENT): Use Pattern to validate non-missing values.
        // TODO(STUDENT): Ignore missing values because MissingValueRule handles them.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
