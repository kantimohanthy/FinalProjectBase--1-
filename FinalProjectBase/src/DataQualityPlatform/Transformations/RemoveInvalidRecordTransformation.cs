using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

public class RemoveInvalidRecordTransformation : IDataTransformation
{
    private readonly List<QualityIssue> _issues;

    public RemoveInvalidRecordTransformation(IEnumerable<QualityIssue> issues)
    {
        _issues = issues.ToList();
    }

    public string Name => "Remove Invalid Records";

    public Dataset Apply(Dataset dataset)
    {
        // TODO(STUDENT): Remove records containing at least one Error quality issue.
        // TODO(STUDENT): Keep records that only have Warning or Information issues.
        // TODO(STUDENT): Return a Dataset while avoiding unexpected side effects.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
