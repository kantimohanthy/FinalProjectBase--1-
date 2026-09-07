using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;
using DataQualityPlatform.Observers;
using DataQualityPlatform.Transformations;

namespace DataQualityPlatform.Services;

public class ProcessingPipeline
{
    private readonly List<IDataTransformation> _transformations = new();
    private readonly List<IPipelineObserver> _observers = new();

    public void AddTransformation(IDataTransformation transformation)
    {
        _transformations.Add(transformation);
    }

    public void AddObserver(IPipelineObserver observer)
    {
        _observers.Add(observer);
    }

    public Dataset Execute(Dataset dataset)
    {
        // TODO(STUDENT): Execute transformations in insertion order.
        // TODO(STUDENT): Notify observers when each step starts.
        // TODO(STUDENT): Notify observers when each step completes successfully.
        // TODO(STUDENT): Notify observers when each step fails.
        // TODO(STUDENT): Wrap transformation failures in PipelineExecutionException.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
