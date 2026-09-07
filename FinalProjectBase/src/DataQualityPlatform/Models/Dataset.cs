namespace DataQualityPlatform.Models;

public class Dataset
{
    public string Name { get; set; } = string.Empty;

    public List<ColumnDefinition> Columns { get; set; } = new();

    public List<DataRecord> Records { get; set; } = new();
}
