using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

public class DateFormatRule : QualityRule
{
    public DateFormatRule(string columnName, bool isCritical = false)
        : base("Date Format Rule", columnName, isCritical)
    {
    }

    public string RequiredFormat => "yyyy-MM-dd";

    public override List<QualityIssue> Evaluate(Dataset dataset)
    {
        // TODO(STUDENT): Throw ColumnNotFoundException when ColumnName is not present.
        // TODO(STUDENT): Use DateTime.TryParseExact with the yyyy-MM-dd format.
        // TODO(STUDENT): Ignore missing values because MissingValueRule handles them.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
