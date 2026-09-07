using DataQualityPlatform.DataSources;
using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Models;

namespace DataQualityPlatform.Tests;

public class CsvDataSourceTests
{
    [Fact]
    public void Load_ShouldCreateOneRecordPerCsvLine()
    {
        CsvDataSource source = new();

        Dataset dataset = source.Load(GetDataPath("valid-customers.csv"));

        Assert.Equal(3, dataset.Records.Count);
    }

    [Fact]
    public void Load_ShouldUseFirstLineAsColumnNames()
    {
        CsvDataSource source = new();

        Dataset dataset = source.Load(GetDataPath("valid-customers.csv"));

        Assert.Equal(
            ["CustomerId", "Name", "Email", "Age", "Country", "SignupDate", "TotalSpent"],
            dataset.Columns.Select(column => column.Name));
    }

    [Fact]
    public void Load_ShouldKeepInvalidValuesAsRawStrings()
    {
        CsvDataSource source = new();

        Dataset dataset = source.Load(GetDataPath("invalid-age.csv"));

        Assert.Equal("250", dataset.Records.Single().Values["Age"]);
    }

    [Fact]
    public void Load_ShouldThrowDatasetNotFoundException()
    {
        CsvDataSource source = new();

        Assert.Throws<DatasetNotFoundException>(() => source.Load("does-not-exist.csv"));
    }

    [Fact]
    public void Load_ShouldThrowInvalidDatasetFormatException_WhenRequiredColumnIsMissing()
    {
        CsvDataSource source = new();

        Assert.Throws<InvalidDatasetFormatException>(() => source.Load(GetDataPath("missing-column.csv")));
    }

    [Fact]
    public void Load_ShouldThrowInvalidDatasetFormatException_WhenFileIsEmpty()
    {
        CsvDataSource source = new();

        Assert.Throws<InvalidDatasetFormatException>(() => source.Load(GetDataPath("empty.csv")));
    }

    private static string GetDataPath(string fileName)
    {
        return Path.Combine(AppContext.BaseDirectory, "Data", fileName);
    }
}
