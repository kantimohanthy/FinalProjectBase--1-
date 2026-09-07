using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

public class TrimStringTransformation : IDataTransformation
{
    public string Name => "Trim String Values";

    public Dataset Apply(Dataset dataset)
    {
        // TODO(STUDENT): Trim leading and trailing spaces from every non-null string value.
        // TODO(STUDENT): Return a Dataset while avoiding unexpected side effects.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
