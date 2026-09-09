using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;
using DataQualityPlatform.Observers;
using DataQualityPlatform.Transformations;

namespace DataQualityPlatform.Services;

public class ProcessingPipeline
{
    private readonly List<IDataTransformation> _transformations = new();
    private readonly List<IPipelineObserver> _observers = new();

    public IReadOnlyList<string> TransformationNames =>
        _transformations
            .Select(transformation => transformation.Name)
            .ToList();

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
        Dataset currentDataset = dataset;

        foreach (IDataTransformation transformation in _transformations)
        {
            foreach (IPipelineObserver observer in _observers)
            {
                observer.OnStepStarted(transformation.Name);
            }

            try
            {
                currentDataset = transformation.Apply(currentDataset);

                foreach (IPipelineObserver observer in _observers)
                {
                    observer.OnStepCompleted(
                        transformation.Name,
                        currentDataset.Records.Count);
                }
            }
            catch (Exception exception)
            {
                foreach (IPipelineObserver observer in _observers)
                {
                    observer.OnStepFailed(
                        transformation.Name,
                        exception);
                }

                throw new PipelineExecutionException(
                    $"Pipeline transformation failed: {transformation.Name}",
                    exception);
            }
        }

        return currentDataset;
    }
}