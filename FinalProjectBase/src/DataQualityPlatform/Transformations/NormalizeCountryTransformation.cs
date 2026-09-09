using DataQualityPlatform.Models;

namespace DataQualityPlatform.Transformations;

public class NormalizeCountryTransformation : IDataTransformation
{
    public string Name => "Normalize Country Values";

    public Dataset Apply(Dataset dataset)
    {
        Dataset transformed = new()
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
                    Values = new Dictionary<string, string?>(record.Values)
                })
                .ToList()
        };

        foreach (DataRecord record in transformed.Records)
        {
            if (!record.Values.TryGetValue("Country", out string? country) ||
                country is null)
            {
                continue;
            }

            string trimmedCountry = country.Trim();

            record.Values["Country"] = trimmedCountry.ToUpperInvariant() switch
            {
                "FR" or "FRANCE" => "France",
                "UK" or "GREAT BRITAIN" => "United Kingdom",
                "DE" or "DEUTSCHLAND" => "Germany",
                "ES" or "ESPANA" => "Spain",
                "IT" or "ITALIA" => "Italy",
                "NL" or "HOLLAND" => "Netherlands",
                "BE" or "BELGIQUE" => "Belgium",
                "US" or "USA" or "U.S.A." => "United States",
                "CA" => "Canada",
                _ => trimmedCountry
            };
        }

        return transformed;
    }
}