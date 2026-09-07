using DataQualityPlatform.Models;

namespace DataQualityPlatform.DataSources;

/// <summary>
/// Loads datasets from an external source.
/// </summary>
public interface IDataSource
{
    Dataset Load(string path);
}
