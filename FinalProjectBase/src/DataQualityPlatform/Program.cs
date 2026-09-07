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

ApplicationConfiguration configuration = ApplicationConfiguration.Instance;

DbContextOptions<DataQualityContext> options = new DbContextOptionsBuilder<DataQualityContext>()
    .UseSqlite("Data Source=data-quality-platform.db")
    .Options;

DataQualityContext context = new(options);
IProcessingRunRepository repository = new EfProcessingRunRepository(context);

ProcessingPipeline pipeline = new();
pipeline.AddObserver(new ConsolePipelineObserver());
pipeline.AddTransformation(new TrimStringTransformation());
pipeline.AddTransformation(new NormalizeCountryTransformation());

List<IQualityRule> rules =
[
    new MissingValueRule("CustomerId", true),
    new MissingValueRule("Email", true),
    new UniqueValueRule("CustomerId", true),
    new RangeRule("Age", 0, 120, true),
    new RegexRule("Email", @"^[^@\s]+@[^@\s]+\.[^@\s]+$", true),
    new DateFormatRule("SignupDate", true)
];

List<IDataExporter> exporters =
[
    new CsvDataExporter(),
    new JsonDataExporter()
];

DataProcessingController controller = new(
    new CsvDataSource(),
    new ConsoleView(),
    new DatasetProfiler(),
    new QualityAnalyzer(),
    pipeline,
    new QualityScoreCalculator(),
    repository,
    rules,
    exporters,
    new TransformationHistory());

controller.Run();




