namespace DataQualityPlatform.Exceptions;

public class DatasetNotFoundException : Exception
{
    public DatasetNotFoundException()
    {
    }

    public DatasetNotFoundException(string message)
        : base(message)
    {
    }

    public DatasetNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
