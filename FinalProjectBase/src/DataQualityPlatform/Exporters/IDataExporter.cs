using DataQualityPlatform.Models;

namespace DataQualityPlatform.Exporters;

/// <summary>
/// Exports a dataset to a specific external format.
/// </summary>
public interface IDataExporter
{
    string FormatName { get; }

    void Export(Dataset dataset, string path);
}
