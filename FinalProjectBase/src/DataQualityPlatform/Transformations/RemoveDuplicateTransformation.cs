using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

public class RemoveDuplicateTransformation : IDataTransformation
{
    public RemoveDuplicateTransformation(string columnName)
    {
        ColumnName = columnName;
    }

    public string Name => "Remove Duplicate Records";

    public string ColumnName { get; }

    public Dataset Apply(Dataset dataset)
    {
        // TODO(STUDENT): Keep the first record for each value in ColumnName.
        // TODO(STUDENT): Remove later duplicate records.
        // TODO(STUDENT): Preserve the original record order.
        // TODO(STUDENT): Return a Dataset while avoiding unexpected side effects.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
