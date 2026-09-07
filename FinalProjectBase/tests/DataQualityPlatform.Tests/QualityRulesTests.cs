using DataQualityPlatform.Models;
using DataQualityPlatform.Quality;
using DataQualityPlatform.Tests.Helpers;

namespace DataQualityPlatform.Tests;

public class QualityRulesTests
{
    [Fact]
    public void MissingValueRule_ShouldDetectNullValue()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Email", null));
        MissingValueRule rule = new("Email");

        List<QualityIssue> issues = rule.Evaluate(dataset);

        QualityIssue issue = Assert.Single(issues);
        Assert.Equal(1, issue.RowNumber);
        Assert.Equal("Email", issue.ColumnName);
    }

    [Fact]
    public void MissingValueRule_ShouldDetectWhitespaceValue()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Name", "   "));
        MissingValueRule rule = new("Name");

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Single(issues);
    }

    [Fact]
    public void UniqueValueRule_ShouldDetectOnlyLaterOccurrences()
    {
        Dataset dataset = DatasetTestFactory.CreateRecords(
            new Dictionary<string, string?> { ["CustomerId"] = "1" },
            new Dictionary<string, string?> { ["CustomerId"] = "2" },
            new Dictionary<string, string?> { ["CustomerId"] = "1" },
            new Dictionary<string, string?> { ["CustomerId"] = "1" });
        UniqueValueRule rule = new("CustomerId");

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Equal([3, 4], issues.Select(issue => issue.RowNumber));
    }

    [Fact]
    public void RangeRule_ShouldAcceptInclusiveLimits()
    {
        Dataset dataset = DatasetTestFactory.CreateRecords(
            new Dictionary<string, string?> { ["Age"] = "18" },
            new Dictionary<string, string?> { ["Age"] = "65" });
        RangeRule rule = new("Age", 18, 65);

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Empty(issues);
    }

    [Fact]
    public void RangeRule_ShouldDetectValueBelowMinimum()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Age", "17"));
        RangeRule rule = new("Age", 18, 65);

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Single(issues);
    }

    [Fact]
    public void RangeRule_ShouldDetectValueAboveMaximum()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Age", "66"));
        RangeRule rule = new("Age", 18, 65);

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Single(issues);
    }

    [Fact]
    public void RangeRule_ShouldDetectNonNumericValue()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Age", "not-a-number"));
        RangeRule rule = new("Age", 18, 65);

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Single(issues);
    }

    [Fact]
    public void RegexRule_ShouldDetectInvalidEmail()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("Email", "invalid-email"));
        RegexRule rule = new("Email", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Single(issues);
    }

    [Fact]
    public void DateFormatRule_ShouldDetectInvalidIsoDate()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("SignupDate", "01/15/2025"));
        DateFormatRule rule = new("SignupDate");

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Single(issues);
    }

    [Fact]
    public void DateFormatRule_ShouldAcceptValidIsoDate()
    {
        Dataset dataset = DatasetTestFactory.CreateSingleRecord(("SignupDate", "2025-01-15"));
        DateFormatRule rule = new("SignupDate");

        List<QualityIssue> issues = rule.Evaluate(dataset);

        Assert.Empty(issues);
    }
}
