namespace DataQualityPlatform.Models;

public class DataRecord
{
    public int RowNumber { get; set; }

    public Dictionary<string, string?> Values { get; set; } = new();
}
