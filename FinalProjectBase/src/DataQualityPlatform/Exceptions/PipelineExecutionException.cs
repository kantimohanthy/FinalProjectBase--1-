namespace DataQualityPlatform.Exceptions;

public class PipelineExecutionException : Exception
{
    public PipelineExecutionException()
    {
    }

    public PipelineExecutionException(string message)
        : base(message)
    {
    }

    public PipelineExecutionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
