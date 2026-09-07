namespace DataQualityPlatform.Models;

public class DatasetProfile
{
    public string DatasetName { get; set; } = string.Empty;

    public int RowCount { get; set; }

    public int ColumnCount { get; set; }

    public Dictionary<string, Dictionary<string, string?>> ColumnStatistics { get; set; } = new();
}
