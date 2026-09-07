using DataQualityPlatform.Models;

namespace DataQualityPlatform.Exporters;

public class CsvDataExporter : IDataExporter
{
    public string FormatName => "CSV";

    public void Export(Dataset dataset, string path)
    {
        // TODO(STUDENT): Write column headers.
        // TODO(STUDENT): Write one output row per DataRecord.
        // TODO(STUDENT): Keep the simple comma-separated format used by this project.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
