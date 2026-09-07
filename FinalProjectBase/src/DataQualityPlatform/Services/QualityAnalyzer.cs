using DataQualityPlatform.Models;
using DataQualityPlatform.Quality;

namespace DataQualityPlatform.Services;

public class QualityAnalyzer
{
    public List<QualityIssue> Analyze(
        Dataset dataset,
        IEnumerable<IQualityRule> rules)
    {
        // TODO(STUDENT): Evaluate each rule against the dataset.
        // TODO(STUDENT): Combine all returned QualityIssue objects into one list.
        // TODO(STUDENT): Do not swallow exceptions from rules.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
