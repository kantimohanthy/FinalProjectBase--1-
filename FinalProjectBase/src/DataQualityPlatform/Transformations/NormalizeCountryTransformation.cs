using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

public class NormalizeCountryTransformation : IDataTransformation
{
    public string Name => "Normalize Country Values";

    public Dataset Apply(Dataset dataset)
    {
        // TODO(STUDENT): Trim the country value before normalization.
        // TODO(STUDENT): Compare country values case-insensitively.
        // TODO(STUDENT): Normalize fr, FR, and FRANCE to France.
        // TODO(STUDENT): Normalize UK, uk, and Great Britain to United Kingdom.
        // TODO(STUDENT): Normalize DE, de, and Deutschland to Germany.
        // TODO(STUDENT): Normalize ES and Espana to Spain.
        // TODO(STUDENT): Normalize IT and Italia to Italy.
        // TODO(STUDENT): Normalize NL and Holland to Netherlands.
        // TODO(STUDENT): Normalize BE and Belgique to Belgium.
        // TODO(STUDENT): Normalize US, USA, and U.S.A. to United States.
        // TODO(STUDENT): Normalize CA to Canada.
        // TODO(STUDENT): Keep unknown countries unchanged.
        // TODO(STUDENT): Return a Dataset while avoiding unexpected side effects.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
