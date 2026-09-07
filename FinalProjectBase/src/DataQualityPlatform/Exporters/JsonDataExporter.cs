using DataQualityPlatform.Models;

namespace DataQualityPlatform.Exporters;

public class JsonDataExporter : IDataExporter
{
    public string FormatName => "JSON";

    public void Export(Dataset dataset, string path)
    {
        // TODO(STUDENT): Serialize the Dataset to JSON.
        // TODO(STUDENT): Include columns and records in the exported file.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
