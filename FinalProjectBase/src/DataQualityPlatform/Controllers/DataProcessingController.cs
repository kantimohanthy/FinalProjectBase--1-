using DataQualityPlatform.DataSources;
using DataQualityPlatform.Exporters;
using DataQualityPlatform.Models;
using DataQualityPlatform.Persistence.Entities;
using DataQualityPlatform.Quality;
using DataQualityPlatform.Repositories;
using DataQualityPlatform.Services;
using DataQualityPlatform.Views;

namespace DataQualityPlatform.Controllers;

public class DataProcessingController
{
    private readonly IDataSource _dataSource;
    private readonly ConsoleView _view;
    private readonly DatasetProfiler _datasetProfiler;
    private readonly QualityAnalyzer _qualityAnalyzer;
    private readonly ProcessingPipeline _processingPipeline;
    private readonly QualityScoreCalculator _qualityScoreCalculator;
    private readonly IProcessingRunRepository _processingRunRepository;
    private readonly IEnumerable<IQualityRule> _qualityRules;
    private readonly IEnumerable<IDataExporter> _dataExporters;
    private readonly TransformationHistory _transformationHistory;

    private Dataset? _currentDataset;
    private Dataset? _originalDataset;
    private List<QualityIssue> _currentIssues = new();
    private List<string> _appliedTransformations = new();

    public DataProcessingController(
        IDataSource dataSource,
        ConsoleView view,
        DatasetProfiler datasetProfiler,
        QualityAnalyzer qualityAnalyzer,
        ProcessingPipeline processingPipeline,
        QualityScoreCalculator qualityScoreCalculator,
        IProcessingRunRepository processingRunRepository,
        IEnumerable<IQualityRule> qualityRules,
        IEnumerable<IDataExporter> dataExporters,
        TransformationHistory transformationHistory)
    {
        _dataSource = dataSource;
        _view = view;
        _datasetProfiler = datasetProfiler;
        _qualityAnalyzer = qualityAnalyzer;
        _processingPipeline = processingPipeline;
        _qualityScoreCalculator = qualityScoreCalculator;
        _processingRunRepository = processingRunRepository;
        _qualityRules = qualityRules;
        _dataExporters = dataExporters;
        _transformationHistory = transformationHistory;
    }

    public void Run()
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            _view.DisplayMenu();
            string choice = _view.ReadMenuChoice();

            try
            {
                switch (choice)
                {
                    case "1":
                        LoadDataset();
                        break;
                    case "2":
                        ShowPreview();
                        break;
                    case "3":
                        ShowProfile();
                        break;
                    case "4":
                        RunQualityChecks();
                        break;
                    case "5":
                        ApplyTransformations();
                        break;
                    case "6":
                        ShowQualityReport();
                        break;
                    case "7":
                        ExportDataset();
                        break;
                    case "8":
                        ShowExecutionHistory();
                        break;
                    case "9":
                        UndoLastTransformation();
                        break;
                    case "0":
                        exitRequested = true;
                        break;
                    default:
                        _view.DisplayError("Unknown menu option.");
                        break;
                }
            }
            catch (Exception exception)
            {
                _view.DisplayError(exception.Message);
            }
        }
    }

    private void LoadDataset()
    {
        string path = _view.ReadFilePath();

        _currentDataset = _dataSource.Load(path);
        _originalDataset = CloneDataset(_currentDataset);
        _currentIssues.Clear();
        _appliedTransformations.Clear();

        _view.DisplayMessage(
            $"Dataset loaded successfully: {_currentDataset.Name}");
    }

    private void ShowPreview()
    {
        Dataset dataset = RequireDataset();
        _view.DisplayDatasetPreview(dataset);
    }

    private void ShowProfile()
    {
        Dataset dataset = RequireDataset();
        DatasetProfile profile = _datasetProfiler.Profile(dataset);
        _view.DisplayProfile(profile);
    }

    private void RunQualityChecks()
    {
        Dataset dataset = RequireDataset();

        _currentIssues = _qualityAnalyzer.Analyze(
            dataset,
            _qualityRules);

        _view.DisplayMessage(
            $"Quality checks completed. Issues found: {_currentIssues.Count}");
    }

    private void ApplyTransformations()
    {
        Dataset dataset = RequireDataset();

        _transformationHistory.Save(dataset);

        ProcessingRunEntity run = new()
        {
            DatasetName = dataset.Name,
            StartedAt = DateTime.UtcNow,
            Status = "Running",
            InputRecordCount = dataset.Records.Count
        };

        try
        {
            _currentDataset = _processingPipeline.Execute(dataset);

            _appliedTransformations.AddRange(
                _processingPipeline.TransformationNames);

            _currentIssues = _qualityAnalyzer.Analyze(
                _currentDataset,
                _qualityRules);

            run.CompletedAt = DateTime.UtcNow;
            run.Status = "Completed";
            run.OutputRecordCount = _currentDataset.Records.Count;
            run.QualityScore = _qualityScoreCalculator.Calculate(
                _currentDataset,
                _currentIssues);

            run.QualityIssues = _currentIssues
                .Select(issue => new QualityIssueEntity
                {
                    RowNumber = issue.RowNumber,
                    ColumnName = issue.ColumnName,
                    RuleName = issue.RuleName,
                    InvalidValue = issue.InvalidValue,
                    Message = issue.Message,
                    Severity = issue.Severity
                })
                .ToList();

            _processingRunRepository.Add(run);

            _view.DisplayMessage(
                "Transformations completed successfully.");
        }
        catch
        {
            run.CompletedAt = DateTime.UtcNow;
            run.Status = "Failed";
            run.OutputRecordCount = dataset.Records.Count;
            run.QualityScore = 0;

            _processingRunRepository.Add(run);
            throw;
        }
    }

    private void ShowQualityReport()
    {
        Dataset finalDataset = RequireDataset();
        Dataset initialDataset = _originalDataset ?? finalDataset;

        List<QualityIssue> initialIssues =
            _qualityAnalyzer.Analyze(initialDataset, _qualityRules);

        List<QualityIssue> finalIssues =
            _qualityAnalyzer.Analyze(finalDataset, _qualityRules);

        _currentIssues = finalIssues;

        QualityReport report = new()
        {
            DatasetName = finalDataset.Name,
            ExecutionDate = DateTime.UtcNow,
            InitialRecordCount = initialDataset.Records.Count,
            FinalRecordCount = finalDataset.Records.Count,
            InitialScore = _qualityScoreCalculator.Calculate(
                initialDataset,
                initialIssues),
            FinalScore = _qualityScoreCalculator.Calculate(
                finalDataset,
                finalIssues),
            DetectedIssues = finalIssues,
            AppliedTransformations =
                new List<string>(_appliedTransformations)
        };

        _view.DisplayQualityReport(report);
    }

    private void ExportDataset()
    {
        Dataset dataset = RequireDataset();
        List<IDataExporter> exporters = _dataExporters.ToList();

        if (exporters.Count == 0)
        {
            throw new InvalidOperationException(
                "No exporters are configured.");
        }

        _view.DisplayMessage("Available export formats:");

        for (int index = 0; index < exporters.Count; index++)
        {
            _view.DisplayMessage(
                $"{index + 1}. {exporters[index].FormatName}");
        }

        string selectedValue = _view.ReadMenuChoice();

        if (!int.TryParse(selectedValue, out int selection) ||
            selection < 1 ||
            selection > exporters.Count)
        {
            throw new InvalidOperationException(
                "Invalid export format selection.");
        }

        string path = _view.ReadFilePath();
        IDataExporter exporter = exporters[selection - 1];

        exporter.Export(dataset, path);

        _view.DisplayMessage(
            $"{exporter.FormatName} export completed: {path}");
    }

    private void ShowExecutionHistory()
    {
        List<ProcessingRunEntity> runs =
            _processingRunRepository.GetAll();

        if (runs.Count == 0)
        {
            _view.DisplayMessage(
                "No processing history is available.");
            return;
        }

        _view.DisplayExecutionHistory(runs);
    }

    private void UndoLastTransformation()
    {
        if (!_transformationHistory.CanUndo)
        {
            _view.DisplayMessage(
                "There is no transformation available to undo.");
            return;
        }

        _currentDataset = _transformationHistory.Undo();

        _currentIssues = _qualityAnalyzer.Analyze(
            _currentDataset,
            _qualityRules);

        _appliedTransformations.Clear();

        _view.DisplayMessage(
            "The last transformation has been undone.");
    }

    private Dataset RequireDataset()
    {
        return _currentDataset ??
            throw new InvalidOperationException(
                "Load a dataset before selecting this option.");
    }

    private static Dataset CloneDataset(Dataset dataset)
    {
        return new Dataset
        {
            Name = dataset.Name,
            Columns = dataset.Columns
                .Select(column => new ColumnDefinition
                {
                    Name = column.Name,
                    DetectedType = column.DetectedType,
                    IsNullable = column.IsNullable
                })
                .ToList(),
            Records = dataset.Records
                .Select(record => new DataRecord
                {
                    RowNumber = record.RowNumber,
                    Values = new Dictionary<string, string?>(
                        record.Values)
                })
                .ToList()
        };
    }
}