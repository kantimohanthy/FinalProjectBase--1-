using DataQualityPlatform.Models;
using DataQualityPlatform.Quality;

namespace DataQualityPlatform.Factories;

public class QualityRuleFactory
{
    public IQualityRule Create(QualityRuleConfiguration configuration)
    {
        // TODO(STUDENT): Create the correct IQualityRule implementation for configuration.Type.
        // TODO(STUDENT): Validate required configuration values such as Minimum, Maximum, and Pattern.
        // TODO(STUDENT): Throw UnsupportedRuleException when the rule type is not supported.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
