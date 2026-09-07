namespace DataQualityPlatform.Exceptions;

public class ColumnNotFoundException : Exception
{
    public ColumnNotFoundException()
    {
    }

    public ColumnNotFoundException(string message)
        : base(message)
    {
    }

    public ColumnNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
