using System.Globalization;
using DataQualityPlatform.Models;

namespace DataQualityPlatform.Services;

public class DatasetProfiler
{
    public DatasetProfile Profile(Dataset dataset)
    {
        DatasetProfile profile = new()
        {
            DatasetName = dataset.Name,
            RowCount = dataset.Records.Count,
            ColumnCount = dataset.Columns.Count
        };

        foreach (ColumnDefinition column in dataset.Columns)
        {
            List<string?> values = dataset.Records
                .Select(record => record.Values.GetValueOrDefault(column.Name))
                .ToList();

            List<string> nonMissingValues = values
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!)
                .ToList();

            int missingCount = values.Count - nonMissingValues.Count;

            double missingPercentage = values.Count == 0
                ? 0
                : (double)missingCount / values.Count * 100.0;

            DataType detectedType = DetectType(nonMissingValues);

            Dictionary<string, string?> statistics = new()
            {
                ["DetectedType"] = detectedType.ToString(),
                ["MissingCount"] = missingCount.ToString(CultureInfo.InvariantCulture),
                ["MissingPercentage"] = missingPercentage.ToString(
                    "0.00",
                    CultureInfo.InvariantCulture) + "%",
                ["UniqueCount"] = nonMissingValues
                    .Distinct()
                    .Count()
                    .ToString(CultureInfo.InvariantCulture)
            };

            if (detectedType == DataType.Integer ||
                detectedType == DataType.Decimal)
            {
                List<decimal> numericValues = nonMissingValues
                    .Select(value => decimal.Parse(
                        value,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture))
                    .ToList();

                if (numericValues.Count > 0)
                {
                    statistics["Minimum"] = numericValues
                        .Min()
                        .ToString(CultureInfo.InvariantCulture);

                    statistics["Maximum"] = numericValues
                        .Max()
                        .ToString(CultureInfo.InvariantCulture);

                    statistics["Mean"] = numericValues
                        .Average()
                        .ToString("0.##", CultureInfo.InvariantCulture);
                }
            }

            profile.ColumnStatistics[column.Name] = statistics;
        }

        return profile;
    }

    private static DataType DetectType(IEnumerable<string> values)
    {
        List<string> valueList = values.ToList();

        if (valueList.Count == 0)
        {
            return DataType.String;
        }

        if (valueList.All(value => int.TryParse(
                value,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out _)))
        {
            return DataType.Integer;
        }

        if (valueList.All(value => decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out _)))
        {
            return DataType.Decimal;
        }

        if (valueList.All(value => DateTime.TryParse(
                value,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _)))
        {
            return DataType.Date;
        }

        if (valueList.All(value => bool.TryParse(value, out _)))
        {
            return DataType.Boolean;
        }

        return DataType.String;
    }
}