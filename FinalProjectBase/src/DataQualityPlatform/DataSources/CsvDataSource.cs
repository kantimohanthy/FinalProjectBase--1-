using DataQualityPlatform.Models;

namespace DataQualityPlatform.DataSources;

public class CsvDataSource : IDataSource
{
    public static readonly IReadOnlyList<string> RequiredColumns =
    [
        "CustomerId",
        "Name",
        "Email",
        "Age",
        "Country",
        "SignupDate",
        "TotalSpent"
    ];

    public Dataset Load(string path)
    {
        // TODO(STUDENT): Check that the file exists.
        // TODO(STUDENT): Read the headers from the first CSV line.
        // TODO(STUDENT): Validate that all required columns are present.
        // TODO(STUDENT): Convert every CSV row into a DataRecord.
        // TODO(STUDENT): Keep imported values as raw strings, including invalid values.
        // TODO(STUDENT): Do not trim, normalize, reject, deduplicate, or transform data here.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}

