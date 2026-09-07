namespace DataQualityPlatform.Observers;

/// <summary>
/// Receives notifications from a processing pipeline.
/// </summary>
public interface IPipelineObserver
{
    void OnStepStarted(string stepName);

    void OnStepCompleted(
        string stepName,
        int recordCount);

    void OnStepFailed(
        string stepName,
        Exception exception);
}
