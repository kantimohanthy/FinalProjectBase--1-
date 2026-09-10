namespace DataQualityPlatform.DataSources;

/// <summary>
/// Represents an older CSV reader with an interface that is
/// incompatible with IDataSource.
/// </summary>
public class LegacyCsvReader
{
    public string[] ReadFile(string path)
    {
        return File.ReadAllLines(path);
    }
}