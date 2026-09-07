using DataQualityPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataQualityPlatform.Persistence;

public class DataQualityContext : DbContext
{
    public DataQualityContext(DbContextOptions<DataQualityContext> options)
        : base(options)
    {
    }

    public DbSet<ProcessingRunEntity> ProcessingRuns => Set<ProcessingRunEntity>();

    public DbSet<QualityIssueEntity> QualityIssues => Set<QualityIssueEntity>();

    public DbSet<QualityRuleConfigurationEntity> QualityRuleConfigurations => Set<QualityRuleConfigurationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProcessingRunEntity>()
            .HasMany(run => run.QualityIssues)
            .WithOne(issue => issue.ProcessingRun)
            .HasForeignKey(issue => issue.ProcessingRunEntityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
