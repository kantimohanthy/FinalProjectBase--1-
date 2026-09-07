namespace DataQualityPlatform.Models;

public enum DataType
{
    String,
    Integer,
    Decimal,
    Date,
    Boolean
}

public class ColumnDefinition
{
    public string Name { get; set; } = string.Empty;

    public DataType DetectedType { get; set; }

    public bool IsNullable { get; set; }
}
