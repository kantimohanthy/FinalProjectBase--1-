using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class RangeRule : QualityRule
{
    public RangeRule(
        string columnName,
        decimal minimum,
        decimal maximum,
        bool isCritical = false)
        : base("Range Rule", columnName, isCritical)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public decimal Minimum { get; }

    public decimal Maximum { get; }

    public override List<QualityIssue> Evaluate(Dataset dataset)
    {
        // TODO(STUDENT): Throw ColumnNotFoundException when ColumnName is not present.
        // TODO(STUDENT): Parse values with CultureInfo.InvariantCulture.
        // TODO(STUDENT): Ignore missing values because MissingValueRule handles them.
        // TODO(STUDENT): Treat minimum and maximum limits as inclusive.
        // TODO(STUDENT): Generate an issue for non-numeric values.
        // TODO(STUDENT): Generate an issue for values outside the inclusive range.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
