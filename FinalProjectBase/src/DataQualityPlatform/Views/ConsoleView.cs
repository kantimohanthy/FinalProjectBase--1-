using DataQualityPlatform.Models;
using DataQualityPlatform.Persistence.Entities;

namespace DataQualityPlatform.Views;

public class ConsoleView
{
    public void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Data Processing and Quality Platform");
        Console.WriteLine("1. Load dataset");
        Console.WriteLine("2. Show preview");
        Console.WriteLine("3. Show profile");
        Console.WriteLine("4. Run quality checks");
        Console.WriteLine("5. Apply transformations");
        Console.WriteLine("6. Show quality report");
        Console.WriteLine("7. Export dataset");
        Console.WriteLine("8. Show execution history");
        Console.WriteLine("9. Undo last transformation");
        Console.WriteLine("10. Manage quality rules");
        Console.WriteLine("0. Exit");
    }

    public string ReadMenuChoice()
    {
        Console.Write("Select an option: ");
        return (Console.ReadLine() ?? string.Empty).Trim();
    }

    public string ReadFilePath()
    {
        Console.Write("Enter a file path: ");
        return (Console.ReadLine() ?? string.Empty).Trim();
    }

    public string ReadInput(string prompt)
    {
        Console.Write(prompt);
        return (Console.ReadLine() ?? string.Empty).Trim();
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void DisplayError(string message)
    {
        Console.WriteLine($"Error: {message}");
    }

    public void DisplayDatasetPreview(Dataset dataset)
    {
        Console.WriteLine($"Dataset: {dataset.Name}");
        Console.WriteLine($"Rows: {dataset.Records.Count}");

        if (dataset.Columns.Count == 0)
        {
            Console.WriteLine("No columns available.");
            return;
        }

        Console.WriteLine(
            string.Join(
                ", ",
                dataset.Columns.Select(column => column.Name)));

        foreach (DataRecord record in dataset.Records.Take(10))
        {
            IEnumerable<string?> values =
                dataset.Columns.Select(
                    column =>
                        record.Values.GetValueOrDefault(column.Name));

            Console.WriteLine(string.Join(", ", values));
        }
    }

    public void DisplayProfile(DatasetProfile profile)
    {
        Console.WriteLine($"Dataset: {profile.DatasetName}");
        Console.WriteLine($"Rows: {profile.RowCount}");
        Console.WriteLine($"Columns: {profile.ColumnCount}");

        foreach (
            KeyValuePair<string, Dictionary<string, string?>>
                column in profile.ColumnStatistics)
        {
            Console.WriteLine($"Column: {column.Key}");

            foreach (
                KeyValuePair<string, string?>
                    statistic in column.Value)
            {
                Console.WriteLine(
                    $"  {statistic.Key}: {statistic.Value}");
            }
        }
    }

    public void DisplayQualityReport(QualityReport report)
    {
        Console.WriteLine($"Dataset: {report.DatasetName}");
        Console.WriteLine($"Execution date: {report.ExecutionDate}");
        Console.WriteLine(
            $"Initial records: {report.InitialRecordCount}");
        Console.WriteLine(
            $"Final records: {report.FinalRecordCount}");
        Console.WriteLine($"Initial score: {report.InitialScore}");
        Console.WriteLine($"Final score: {report.FinalScore}");
        Console.WriteLine(
            $"Issues: {report.DetectedIssues.Count}");
        Console.WriteLine(
            $"Transformations: {string.Join(", ", report.AppliedTransformations)}");
    }

    public void DisplayExecutionHistory(
        IEnumerable<ProcessingRunEntity> runs)
    {
        List<ProcessingRunEntity> runList = runs.ToList();

        if (runList.Count == 0)
        {
            Console.WriteLine("No processing history available.");
            return;
        }

        foreach (ProcessingRunEntity run in runList)
        {
            Console.WriteLine(
                $"{run.Id}: {run.DatasetName} | " +
                $"{run.Status} | Score: {run.QualityScore} | " +
                $"Started: {run.StartedAt}");
        }
    }

    public void DisplayQualityRuleConfigurations(
        IEnumerable<QualityRuleConfigurationEntity> configurations)
    {
        List<QualityRuleConfigurationEntity> configurationList =
            configurations.ToList();

        if (configurationList.Count == 0)
        {
            Console.WriteLine(
                "No quality-rule configurations available.");
            return;
        }

        foreach (
            QualityRuleConfigurationEntity configuration
                in configurationList)
        {
            Console.WriteLine(
                $"{configuration.Id}: {configuration.Name} | " +
                $"Type: {configuration.RuleType} | " +
                $"Column: {configuration.ColumnName} | " +
                $"Critical: {configuration.IsCritical} | " +
                $"Minimum: {configuration.Minimum} | " +
                $"Maximum: {configuration.Maximum} | " +
                $"Pattern: {configuration.Pattern}");
        }
    }
}