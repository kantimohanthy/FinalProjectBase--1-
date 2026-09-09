using DataQualityPlatform.Models;
using DataQualityPlatform.Quality;

namespace DataQualityPlatform.Services;

public class QualityAnalyzer
{
    public List<QualityIssue> Analyze(
        Dataset dataset,
        IEnumerable<IQualityRule> rules)
    {
        List<QualityIssue> issues = new();

        foreach (IQualityRule rule in rules)
        {
            List<QualityIssue> ruleIssues = rule.Evaluate(dataset);
            issues.AddRange(ruleIssues);
        }

        return issues;
    }
}