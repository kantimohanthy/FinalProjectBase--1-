using DataQualityPlatform.Models;
using DataQualityPlatform.Tests.Helpers;
using DataQualityPlatform.Transformations;

namespace DataQualityPlatform.Tests;

public class TransformationsTests
{
    [Fact]
    public void TrimStringTransformation_ShouldRemoveExternalSpaces()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Name", "  Alice Martin  "));
        TrimStringTransformation transformation = new();

        Dataset transformed = transformation.Apply(dataset);

        Assert.Equal("Alice Martin", transformed.Records.Single().Values["Name"]);
    }

    [Fact]
    public void NormalizeCountryTransformation_ShouldNormalizeFrance()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Country", "fr"));
        NormalizeCountryTransformation transformation = new();

        Dataset transformed = transformation.Apply(dataset);

        Assert.Equal("France", transformed.Records.Single().Values["Country"]);
    }

    [Fact]
    public void NormalizeCountryTransformation_ShouldNormalizeUnitedKingdom()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Country", "Great Britain"));
        NormalizeCountryTransformation transformation = new();

        Dataset transformed = transformation.Apply(dataset);

        Assert.Equal("United Kingdom", transformed.Records.Single().Values["Country"]);
    }

    [Fact]
    public void NormalizeCountryTransformation_ShouldKeepUnknownCountry()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Country", "Atlantis"));
        NormalizeCountryTransformation transformation = new();

        Dataset transformed = transformation.Apply(dataset);

        Assert.Equal("Atlantis", transformed.Records.Single().Values["Country"]);
    }

    [Fact]
    public void RemoveDuplicateTransformation_ShouldKeepFirstRecord()
    {
        Dataset dataset = DatasetTestFactory.CreateRecords(
            new Dictionary<string, string?> { ["CustomerId"] = "1", ["Name"] = "Alice" },
            new Dictionary<string, string?> { ["CustomerId"] = "1", ["Name"] = "Duplicate Alice" },
            new Dictionary<string, string?> { ["CustomerId"] = "2", ["Name"] = "Bob" });
        RemoveDuplicateTransformation transformation = new("CustomerId");

        Dataset transformed = transformation.Apply(dataset);

        Assert.Equal(["Alice", "Bob"], transformed.Records.Select(record => record.Values["Name"]));
    }

    [Fact]
    public void RemoveDuplicateTransformation_ShouldPreserveOrder()
    {
        Dataset dataset = DatasetTestFactory.CreateRecords(
            new Dictionary<string, string?> { ["CustomerId"] = "2", ["Name"] = "Bob" },
            new Dictionary<string, string?> { ["CustomerId"] = "1", ["Name"] = "Alice" },
            new Dictionary<string, string?> { ["CustomerId"] = "2", ["Name"] = "Duplicate Bob" },
            new Dictionary<string, string?> { ["CustomerId"] = "3", ["Name"] = "Charlie" });
        RemoveDuplicateTransformation transformation = new("CustomerId");

        Dataset transformed = transformation.Apply(dataset);

        Assert.Equal(["Bob", "Alice", "Charlie"], transformed.Records.Select(record => record.Values["Name"]));
    }
}
