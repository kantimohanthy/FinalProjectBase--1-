using DataQualityPlatform.Persistence;
using DataQualityPlatform.Persistence.Entities;
using DataQualityPlatform.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataQualityPlatform.Tests;

public class RepositoryTests
{
    [Fact]
    public void Repository_ShouldAddAndRetrieveProcessingRun()
    {
        using DataQualityContext context = CreateContext();
        EfProcessingRunRepository repository = new(context);
        ProcessingRunEntity run = new()
        {
            DatasetName = "customers.csv",
            StartedAt = DateTime.UtcNow,
            Status = "Completed",
            InputRecordCount = 3,
            OutputRecordCount = 3,
            QualityScore = 100
        };

        repository.Add(run);
        ProcessingRunEntity? retrieved = repository.GetById(run.Id);

        Assert.NotNull(retrieved);
        Assert.Equal("customers.csv", retrieved.DatasetName);
    }

    [Fact]
    public void Repository_ShouldDeleteProcessingRun()
    {
        using DataQualityContext context = CreateContext();
        EfProcessingRunRepository repository = new(context);
        ProcessingRunEntity run = new()
        {
            DatasetName = "customers.csv",
            StartedAt = DateTime.UtcNow,
            Status = "Completed"
        };
        repository.Add(run);

        repository.Delete(run.Id);

        Assert.Null(repository.GetById(run.Id));
    }

    private static DataQualityContext CreateContext()
    {
        DbContextOptions<DataQualityContext> options = new DbContextOptionsBuilder<DataQualityContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DataQualityContext(options);
    }
}
