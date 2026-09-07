using DataQualityPlatform.DataSources;
using DataQualityPlatform.Exporters;
using DataQualityPlatform.Models;
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
    private List<QualityIssue> _currentIssues = new();

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
        // TODO(STUDENT): Ask the view for a file path.
        // TODO(STUDENT): Use IDataSource to load the Dataset.
        // TODO(STUDENT): Store the current Dataset and reset current issues.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    private void ShowPreview()
    {
        // TODO(STUDENT): Validate that a Dataset is loaded.
        // TODO(STUDENT): Delegate preview rendering to ConsoleView.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    private void ShowProfile()
    {
        // TODO(STUDENT): Validate that a Dataset is loaded.
        // TODO(STUDENT): Delegate profiling to DatasetProfiler.
        // TODO(STUDENT): Delegate profile rendering to ConsoleView.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    private void RunQualityChecks()
    {
        // TODO(STUDENT): Validate that a Dataset is loaded.
        // TODO(STUDENT): Delegate rule execution to QualityAnalyzer.
        // TODO(STUDENT): Store the detected QualityIssue list.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    private void ApplyTransformations()
    {
        // TODO(STUDENT): Validate that a Dataset is loaded.
        // TODO(STUDENT): Save the current Dataset in TransformationHistory before transforming.
        // TODO(STUDENT): Delegate transformation execution to ProcessingPipeline.
        // TODO(STUDENT): Store the transformed Dataset.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    private void ShowQualityReport()
    {
        // TODO(STUDENT): Validate that a Dataset is loaded.
        // TODO(STUDENT): Use QualityScoreCalculator for initial and final scores.
        // TODO(STUDENT): Build a QualityReport object and delegate display to ConsoleView.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    private void ExportDataset()
    {
        // TODO(STUDENT): Validate that a Dataset is loaded.
        // TODO(STUDENT): Ask which exporter format should be used.
        // TODO(STUDENT): Delegate writing to the selected IDataExporter.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    private void ShowExecutionHistory()
    {
        // TODO(STUDENT): Retrieve processing runs from IProcessingRunRepository.
        // TODO(STUDENT): Delegate history rendering to ConsoleView.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    private void UndoLastTransformation()
    {
        // TODO(STUDENT): Use TransformationHistory to restore the previous Dataset.
        // TODO(STUDENT): Display a clear message when undo is not available.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}





