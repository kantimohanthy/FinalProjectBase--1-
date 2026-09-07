using DataQualityPlatform.Exceptions;
using DataQualityPlatform.Factories;
using DataQualityPlatform.Models;
using DataQualityPlatform.Quality;

namespace DataQualityPlatform.Tests;

public class QualityRuleFactoryTests
{
    [Fact]
    public void Create_ShouldCreateMissingValueRule()
    {
        QualityRuleFactory factory = new();
        QualityRuleConfiguration configuration = new()
        {
            Type = QualityRuleType.MissingValue,
            ColumnName = "Email",
            IsCritical = true
        };

        IQualityRule rule = factory.Create(configuration);

        Assert.IsType<MissingValueRule>(rule);
    }

    [Fact]
    public void Create_ShouldCreateRangeRule()
    {
        QualityRuleFactory factory = new();
        QualityRuleConfiguration configuration = new()
        {
            Type = QualityRuleType.Range,
            ColumnName = "Age",
            Minimum = 18,
            Maximum = 65,
            IsCritical = true
        };

        IQualityRule rule = factory.Create(configuration);

        Assert.IsType<RangeRule>(rule);
    }

    [Fact]
    public void Create_ShouldThrowUnsupportedRuleException()
    {
        QualityRuleFactory factory = new();
        QualityRuleConfiguration configuration = new()
        {
            Type = (QualityRuleType)999,
            ColumnName = "Email"
        };

        Assert.Throws<UnsupportedRuleException>(() => factory.Create(configuration));
    }
}
