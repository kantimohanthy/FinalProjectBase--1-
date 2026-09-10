using DataQualityPlatform.Configuration;
using DataQualityPlatform.Controllers;
using DataQualityPlatform.DataSources;
using DataQualityPlatform.Exporters;
using DataQualityPlatform.Observers;
using DataQualityPlatform.Persistence;
using DataQualityPlatform.Quality;
using DataQualityPlatform.Repositories;
using DataQualityPlatform.Services;
using DataQualityPlatform.Transformations;
using DataQualityPlatform.Views;
using Microsoft.EntityFrameworkCore;

ApplicationConfiguration configuration =
    ApplicationConfiguration.Instance;

DbContextOptions<DataQualityContext> options =
    new DbContextOptionsBuilder<DataQualityContext>()
        .UseSqlite("Data Source=data-quality-platform.db")
        .Options;

DataQualityContext context = new(options);
context.Database.EnsureCreated();

IProcessingRunRepository processingRunRepository =
    new EfProcessingRunRepository(context);

IQualityRuleRepository qualityRuleRepository =
    new EfQualityRuleRepository(context);

TransformationHistory transformationHistory = new();

ProcessingPipeline pipeline =
    new(transformationHistory);

pipeline.AddObserver(
    new ConsolePipelineObserver());

pipeline.AddTransformation(
    new TrimStringTransformation());

pipeline.AddTransformation(
    new NormalizeCountryTransformation());

pipeline.AddTransformation(
    new RemoveDuplicateTransformation("CustomerId"));

List<IQualityRule> rules =
[
    new MissingValueRule(
        "CustomerId",
        true),

    new MissingValueRule(
        "Email",
        true),

    new UniqueValueRule(
        "CustomerId",
        true),

    new RangeRule(
        "Age",
        0,
        120,
        true),

    new RegexRule(
        "Email",
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        true),

    new DateFormatRule(
        "SignupDate",
        true)
];

List<IDataExporter> exporters =
[
    new CsvDataExporter(),
    new JsonDataExporter()
];

DataProcessingController controller = new(
    new LegacyCsvReaderAdapter(
        new LegacyCsvReader()),
    new ConsoleView(),
    new DatasetProfiler(),
    new QualityAnalyzer(),
    pipeline,
    new QualityScoreCalculator(),
    processingRunRepository,
    qualityRuleRepository,
    rules,
    exporters,
    transformationHistory);

controller.Run();