using DataQualityPlatform.Models;

namespace DataQualityPlatform.Quality;

/// <summary>
/// Evaluates one quality requirement against a dataset.
/// </summary>
public interface IQualityRule
{
    string Name { get; }

    List<QualityIssue> Evaluate(Dataset dataset);
}
