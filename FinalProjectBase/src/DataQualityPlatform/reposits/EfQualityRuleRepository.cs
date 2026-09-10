using DataQualityPlatform.Persistence;
using DataQualityPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataQualityPlatform.Repositories;

public class EfQualityRuleRepository : IQualityRuleRepository
{
    private readonly DataQualityContext _context;

    public EfQualityRuleRepository(
        DataQualityContext context)
    {
        _context = context;
    }

    public void Add(
        QualityRuleConfigurationEntity configuration)
    {
        _context.QualityRuleConfigurations.Add(
            configuration);

        _context.SaveChanges();
    }

    public QualityRuleConfigurationEntity? GetById(int id)
    {
        return _context.QualityRuleConfigurations
            .AsNoTracking()
            .SingleOrDefault(
                configuration =>
                    configuration.Id == id);
    }

    public List<QualityRuleConfigurationEntity> GetAll()
    {
        return _context.QualityRuleConfigurations
            .AsNoTracking()
            .OrderBy(configuration => configuration.Id)
            .ToList();
    }

    public void Update(
        QualityRuleConfigurationEntity configuration)
    {
        QualityRuleConfigurationEntity? existing =
            _context.QualityRuleConfigurations.Local
                .FirstOrDefault(
                    item =>
                        item.Id == configuration.Id);

        existing ??=
            _context.QualityRuleConfigurations.Find(
                configuration.Id);

        if (existing is null)
        {
            throw new InvalidOperationException(
                $"Quality rule configuration " +
                $"{configuration.Id} was not found.");
        }

        existing.Name = configuration.Name;
        existing.RuleType = configuration.RuleType;
        existing.ColumnName = configuration.ColumnName;
        existing.IsCritical = configuration.IsCritical;
        existing.Minimum = configuration.Minimum;
        existing.Maximum = configuration.Maximum;
        existing.Pattern = configuration.Pattern;

        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        QualityRuleConfigurationEntity? configuration =
            _context.QualityRuleConfigurations.Local
                .FirstOrDefault(
                    item =>
                        item.Id == id);

        configuration ??=
            _context.QualityRuleConfigurations.Find(id);

        if (configuration is null)
        {
            return;
        }

        _context.QualityRuleConfigurations.Remove(
            configuration);

        _context.SaveChanges();
    }
}