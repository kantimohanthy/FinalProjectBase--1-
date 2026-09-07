using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

/// <summary>
/// Applies one dataset transformation step.
/// </summary>
public interface IDataTransformation
{
    string Name { get; }

    Dataset Apply(Dataset dataset);
}
