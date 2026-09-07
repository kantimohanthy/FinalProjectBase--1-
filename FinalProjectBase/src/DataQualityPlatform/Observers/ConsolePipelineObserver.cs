namespace DataQualityPlatform.Observers;

public class ConsolePipelineObserver : IPipelineObserver
{
    public void OnStepStarted(string stepName)
    {
        Console.WriteLine($"Starting transformation: {stepName}");
    }

    public void OnStepCompleted(
        string stepName,
        int recordCount)
    {
        Console.WriteLine($"Completed transformation: {stepName}. Records: {recordCount}");
    }

    public void OnStepFailed(
        string stepName,
        Exception exception)
    {
        Console.WriteLine($"Transformation failed: {stepName}. Error: {exception.Message}");
    }
}
