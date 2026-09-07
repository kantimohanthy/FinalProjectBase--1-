using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

/// <summary>
/// Base class for column-oriented quality rules.
/// </summary>
public abstract class QualityRule : IQualityRule
{
    protected QualityRule(
        string name,
        string columnName,
        bool isCritical)
    {
        Name = name;
        ColumnName = columnName;
        IsCritical = isCritical;
    }

    public string Name { get; protected set; }

    public string ColumnName { get; protected set; }

    public bool IsCritical { get; protected set; }

    public abstract List<QualityIssue> Evaluate(Dataset dataset);

    protected string? GetValue(DataRecord record)
    {
        return record.Values.GetValueOrDefault(ColumnName);
    }
}
