using DataQualityPlatform.Models;
using DataQualityPlatform.Services;
using DataQualityPlatform.Tests.Helpers;

namespace DataQualityPlatform.Tests;

public class QualityScoreCalculatorTests
{
    [Fact]
    public void Calculate_ShouldReturnOneHundredWithoutIssues()
    {
        Dataset dataset = DatasetTestFactory.CreateRectangularDataset(2, 2);
        QualityScoreCalculator calculator = new();

        double score = calculator.Calculate(dataset, []);

        Assert.Equal(100, score);
    }

    [Fact]
    public void Calculate_ShouldUseDistinctInvalidCells()
    {
        Dataset dataset = DatasetTestFactory.CreateRectangularDataset(2, 2);
        List<QualityIssue> issues =
        [
            new() { RowNumber = 1, ColumnName = "Column1" },
            new() { RowNumber = 1, ColumnName = "Column1" },
            new() { RowNumber = 2, ColumnName = "Column2" }
        ];
        QualityScoreCalculator calculator = new();

        double score = calculator.Calculate(dataset, issues);

        Assert.Equal(50, score);
    }

    [Fact]
    public void Calculate_ShouldReturnZeroForEmptyDataset()
    {
        Dataset dataset = DatasetTestFactory.CreateRectangularDataset(0, 2);
        QualityScoreCalculator calculator = new();

        double score = calculator.Calculate(dataset, []);

        Assert.Equal(0, score);
    }

    [Fact]
    public void Calculate_ShouldNeverReturnNegativeValue()
    {
        Dataset dataset = DatasetTestFactory.CreateRectangularDataset(1, 1);
        List<QualityIssue> issues =
        [
            new() { RowNumber = 1, ColumnName = "Column1" },
            new() { RowNumber = 2, ColumnName = "Column1" },
            new() { RowNumber = 3, ColumnName = "Column1" }
        ];
        QualityScoreCalculator calculator = new();

        double score = calculator.Calculate(dataset, issues);

        Assert.Equal(0, score);
    }
}
