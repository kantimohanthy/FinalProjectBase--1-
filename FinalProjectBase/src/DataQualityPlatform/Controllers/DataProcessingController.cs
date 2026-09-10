using DataQualityPlatform.DataSources;
using DataQualityPlatform.Exporters;
using DataQualityPlatform.Factories;
using DataQualityPlatform.Models;
using DataQualityPlatform.Persistence.Entities;
using DataQualityPlatform.Quality;
using DataQualityPlatform.Repositories;
using DataQualityPlatform.Services;
using DataQualityPlatform.Transformations;
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
    private readonly IQualityRuleRepository _qualityRuleRepository;
    private readonly List<IQualityRule> _defaultQualityRules;
    private readonly List<IDataExporter> _dataExporters;
    private readonly TransformationHistory _transformationHistory;
    private readonly QualityRuleFactory _qualityRuleFactory = new();

    private Dataset? _currentDataset;
    private Dataset? _originalDataset;
    private List<QualityIssue> _currentIssues = new();
    private readonly List<string> _appliedTransformations = new();

    public DataProcessingController(
        IDataSource dataSource,
        ConsoleView view,
        DatasetProfiler datasetProfiler,
        QualityAnalyzer qualityAnalyzer,
        ProcessingPipeline processingPipeline,
        QualityScoreCalculator qualityScoreCalculator,
        IProcessingRunRepository processingRunRepository,
        IQualityRuleRepository qualityRuleRepository,
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
        _qualityRuleRepository = qualityRuleRepository;
        _defaultQualityRules = qualityRules.ToList();
        _dataExporters = dataExporters.ToList();
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

                    case "10":
                        ManageQualityRules();
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
        List<IQualityRule> rules = GetActiveQualityRules();

        _currentIssues = _qualityAnalyzer.Analyze(dataset, rules);

        _view.DisplayMessage(
            $"Quality checks completed. Issues found: {_currentIssues.Count}");
    }

    private void ApplyTransformations()
    {
        Dataset dataset = RequireDataset();
        List<IQualityRule> rules = GetActiveQualityRules();

        ProcessingRunEntity run = new()
        {
            DatasetName = dataset.Name,
            StartedAt = DateTime.UtcNow,
            Status = "Running",
            InputRecordCount = dataset.Records.Count
        };

        try
        {
            _currentIssues = _qualityAnalyzer.Analyze(dataset, rules);

            Dataset transformedDataset =
                _processingPipeline.Execute(dataset);

            _appliedTransformations.AddRange(
                _processingPipeline.TransformationNames);

            RemoveInvalidRecordTransformation removeInvalid =
                new(_currentIssues);

            _transformationHistory.Save(transformedDataset);

            _currentDataset =
                removeInvalid.Apply(transformedDataset);

            _appliedTransformations.Add(removeInvalid.Name);

            _currentIssues = _qualityAnalyzer.Analyze(
                _currentDataset,
                rules);

            run.CompletedAt = DateTime.UtcNow;
            run.Status = "Completed";
            run.OutputRecordCount =
                _currentDataset.Records.Count;

            run.QualityScore =
                _qualityScoreCalculator.Calculate(
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
        Dataset initialDataset =
            _originalDataset ?? finalDataset;

        List<IQualityRule> rules = GetActiveQualityRules();

        List<QualityIssue> initialIssues =
            _qualityAnalyzer.Analyze(initialDataset, rules);

        List<QualityIssue> finalIssues =
            _qualityAnalyzer.Analyze(finalDataset, rules);

        _currentIssues = finalIssues;

        QualityReport report = new()
        {
            DatasetName = finalDataset.Name,
            ExecutionDate = DateTime.UtcNow,
            InitialRecordCount =
                initialDataset.Records.Count,
            FinalRecordCount =
                finalDataset.Records.Count,
            InitialScore =
                _qualityScoreCalculator.Calculate(
                    initialDataset,
                    initialIssues),
            FinalScore =
                _qualityScoreCalculator.Calculate(
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

        if (_dataExporters.Count == 0)
        {
            throw new InvalidOperationException(
                "No exporters are configured.");
        }

        _view.DisplayMessage("Available export formats:");

        for (int index = 0;
             index < _dataExporters.Count;
             index++)
        {
            _view.DisplayMessage(
                $"{index + 1}. " +
                $"{_dataExporters[index].FormatName}");
        }

        string selectedValue =
            _view.ReadInput("Select an export format: ");

        if (!int.TryParse(
                selectedValue,
                out int selection) ||
            selection < 1 ||
            selection > _dataExporters.Count)
        {
            throw new InvalidOperationException(
                "Invalid export format selection.");
        }

        string path = _view.ReadFilePath();

        IDataExporter exporter =
            _dataExporters[selection - 1];

        exporter.Export(dataset, path);

        _view.DisplayMessage(
            $"{exporter.FormatName} export completed: {path}");
    }

    private void ShowExecutionHistory()
    {
        List<ProcessingRunEntity> runs =
            _processingRunRepository.GetAll();

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
            GetActiveQualityRules());

        if (_appliedTransformations.Count > 0)
        {
            _appliedTransformations.RemoveAt(
                _appliedTransformations.Count - 1);
        }

        _view.DisplayMessage(
            "The last transformation has been undone.");
    }

    private void ManageQualityRules()
    {
        bool returnToMainMenu = false;

        while (!returnToMainMenu)
        {
            _view.DisplayMessage("");
            _view.DisplayMessage("Quality Rule Management");
            _view.DisplayMessage("1. List rules");
            _view.DisplayMessage("2. Add rule");
            _view.DisplayMessage("3. Update rule");
            _view.DisplayMessage("4. Delete rule");
            _view.DisplayMessage("0. Return to main menu");

            string choice =
                _view.ReadInput("Select an option: ");

            switch (choice)
            {
                case "1":
                    ListQualityRules();
                    break;

                case "2":
                    AddQualityRule();
                    break;

                case "3":
                    UpdateQualityRule();
                    break;

                case "4":
                    DeleteQualityRule();
                    break;

                case "0":
                    returnToMainMenu = true;
                    break;

                default:
                    _view.DisplayError(
                        "Unknown quality-rule option.");
                    break;
            }
        }
    }

    private void ListQualityRules()
    {
        List<QualityRuleConfigurationEntity> configurations =
            _qualityRuleRepository.GetAll();

        _view.DisplayQualityRuleConfigurations(
            configurations);
    }

    private void AddQualityRule()
    {
        QualityRuleConfigurationEntity configuration =
            ReadQualityRuleConfiguration();

        ValidateQualityRuleConfiguration(configuration);

        _qualityRuleRepository.Add(configuration);

        _view.DisplayMessage(
            $"Quality rule added with ID {configuration.Id}.");
    }

    private void UpdateQualityRule()
    {
        int id = ReadPositiveInteger(
            "Enter the rule ID to update: ");

        QualityRuleConfigurationEntity existing =
            _qualityRuleRepository.GetById(id) ??
            throw new InvalidOperationException(
                $"Quality rule {id} was not found.");

        _view.DisplayMessage(
            "Enter the replacement configuration.");

        QualityRuleConfigurationEntity replacement =
            ReadQualityRuleConfiguration();

        replacement.Id = existing.Id;

        ValidateQualityRuleConfiguration(replacement);

        _qualityRuleRepository.Update(replacement);

        _view.DisplayMessage(
            $"Quality rule {id} updated.");
    }

    private void DeleteQualityRule()
    {
        int id = ReadPositiveInteger(
            "Enter the rule ID to delete: ");

        QualityRuleConfigurationEntity? existing =
            _qualityRuleRepository.GetById(id);

        if (existing is null)
        {
            throw new InvalidOperationException(
                $"Quality rule {id} was not found.");
        }

        _qualityRuleRepository.Delete(id);

        _view.DisplayMessage(
            $"Quality rule {id} deleted.");
    }

    private QualityRuleConfigurationEntity
        ReadQualityRuleConfiguration()
    {
        string name =
            _view.ReadInput("Rule name: ");

        _view.DisplayMessage(
            "Available types: MissingValue, UniqueValue, " +
            "Range, Regex, DateFormat");

        string typeValue =
            _view.ReadInput("Rule type: ");

        if (!Enum.TryParse(
                typeValue,
                true,
                out QualityRuleType ruleType) ||
            !Enum.IsDefined(ruleType))
        {
            throw new InvalidOperationException(
                "Invalid quality-rule type.");
        }

        string columnName =
            _view.ReadInput("Column name: ");

        if (string.IsNullOrWhiteSpace(columnName))
        {
            throw new InvalidOperationException(
                "Column name is required.");
        }

        string criticalValue =
            _view.ReadInput(
                "Is this rule critical? (yes/no): ");

        bool isCritical =
            criticalValue.Equals(
                "yes",
                StringComparison.OrdinalIgnoreCase) ||
            criticalValue.Equals(
                "y",
                StringComparison.OrdinalIgnoreCase) ||
            criticalValue.Equals(
                "true",
                StringComparison.OrdinalIgnoreCase);

        decimal? minimum = null;
        decimal? maximum = null;
        string? pattern = null;

        if (ruleType == QualityRuleType.Range)
        {
            minimum = ReadDecimal("Minimum value: ");
            maximum = ReadDecimal("Maximum value: ");
        }

        if (ruleType == QualityRuleType.Regex)
        {
            pattern = _view.ReadInput(
                "Regular-expression pattern: ");

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new InvalidOperationException(
                    "A pattern is required for a Regex rule.");
            }
        }

        return new QualityRuleConfigurationEntity
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? ruleType.ToString()
                : name,
            RuleType = ruleType,
            ColumnName = columnName,
            IsCritical = isCritical,
            Minimum = minimum,
            Maximum = maximum,
            Pattern = pattern
        };
    }

    private void ValidateQualityRuleConfiguration(
        QualityRuleConfigurationEntity entity)
    {
        _qualityRuleFactory.Create(
            ToModelConfiguration(entity));
    }

    private List<IQualityRule> GetActiveQualityRules()
    {
        List<QualityRuleConfigurationEntity> configurations =
            _qualityRuleRepository.GetAll();

        if (configurations.Count == 0)
        {
            return new List<IQualityRule>(
                _defaultQualityRules);
        }

        return configurations
            .Select(configuration =>
                _qualityRuleFactory.Create(
                    ToModelConfiguration(configuration)))
            .ToList();
    }

    private static QualityRuleConfiguration
        ToModelConfiguration(
            QualityRuleConfigurationEntity entity)
    {
        return new QualityRuleConfiguration
        {
            Type = entity.RuleType,
            ColumnName = entity.ColumnName,
            IsCritical = entity.IsCritical,
            Minimum = entity.Minimum,
            Maximum = entity.Maximum,
            Pattern = entity.Pattern
        };
    }

    private int ReadPositiveInteger(string prompt)
    {
        string value = _view.ReadInput(prompt);

        if (!int.TryParse(value, out int result) ||
            result <= 0)
        {
            throw new InvalidOperationException(
                "Enter a valid positive integer.");
        }

        return result;
    }

    private decimal ReadDecimal(string prompt)
    {
        string value = _view.ReadInput(prompt);

        if (!decimal.TryParse(
                value,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal result))
        {
            throw new InvalidOperationException(
                "Enter a valid decimal number.");
        }

        return result;
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
                    Values =
                        new Dictionary<string, string?>(
                            record.Values)
                })
                .ToList()
        };
    }
}