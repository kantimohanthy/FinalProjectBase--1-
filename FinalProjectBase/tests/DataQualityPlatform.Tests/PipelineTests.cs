using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;
using DataQualityPlatform.Observers;
using DataQualityPlatform.Services;
using DataQualityPlatform.Tests.Helpers;
using DataQualityPlatform.Transformations;

namespace DataQualityPlatform.Tests;

public class PipelineTests
{
    [Fact]
    public void Pipeline_ShouldExecuteTransformationsInOrder()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Name", "Alice"));
        List<string> order = new();
        ProcessingPipeline pipeline = new();
        pipeline.AddTransformation(new TrackingTransformation("First", order));
        pipeline.AddTransformation(new TrackingTransformation("Second", order));

        Dataset result = pipeline.Execute(dataset);

        Assert.Same(dataset, result);
        Assert.Equal(["First", "Second"], order);
    }

    [Fact]
    public void Pipeline_ShouldNotifyObserver()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Name", "Alice"));
        RecordingObserver observer = new();
        ProcessingPipeline pipeline = new();
        pipeline.AddObserver(observer);
        pipeline.AddTransformation(new TrackingTransformation("Trim", new List<string>()));

        pipeline.Execute(dataset);

        Assert.Contains("Started:Trim", observer.Events);
        Assert.Contains("Completed:Trim", observer.Events);
    }

    [Fact]
    public void Pipeline_ShouldWrapTransformationFailure()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Name", "Alice"));
        ProcessingPipeline pipeline = new();
        pipeline.AddTransformation(new FailingTransformation());

        Assert.Throws<PipelineExecutionException>(() => pipeline.Execute(dataset));
    }

    private sealed class TrackingTransformation : IDataTransformation
    {
        private readonly List<string> _order;

        public TrackingTransformation(string name, List<string> order)
        {
            Name = name;
            _order = order;
        }

        public string Name { get; }

        public Dataset Apply(Dataset dataset)
        {
            _order.Add(Name);
            return dataset;
        }
    }

    private sealed class FailingTransformation : IDataTransformation
    {
        public string Name => "Failing Step";

        public Dataset Apply(Dataset dataset)
        {
            throw new InvalidOperationException("Expected test failure.");
        }
    }

    private sealed class RecordingObserver : IPipelineObserver
    {
        public List<string> Events { get; } = new();

        public void OnStepStarted(string stepName)
        {
            Events.Add($"Started:{stepName}");
        }

        public void OnStepCompleted(
            string stepName,
            int recordCount)
        {
            Events.Add($"Completed:{stepName}");
        }

        public void OnStepFailed(
            string stepName,
            Exception exception)
        {
            Events.Add($"Failed:{stepName}");
        }
    }
}
