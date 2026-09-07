namespace DataQualityPlatform.Exceptions;

public class InvalidDatasetFormatException : Exception
{
    public InvalidDatasetFormatException()
    {
    }

    public InvalidDatasetFormatException(string message)
        : base(message)
    {
    }

    public InvalidDatasetFormatException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
