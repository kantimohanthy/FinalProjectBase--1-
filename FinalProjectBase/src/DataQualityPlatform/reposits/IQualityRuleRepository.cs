using DataQualityPlatform.Persistence.Entities;

namespace DataQualityPlatform.Repositories;

/// <summary>
/// Stores and retrieves quality-rule configurations.
/// </summary>
public interface IQualityRuleRepository
{
    void Add(QualityRuleConfigurationEntity configuration);

    QualityRuleConfigurationEntity? GetById(int id);

    List<QualityRuleConfigurationEntity> GetAll();

    void Update(QualityRuleConfigurationEntity configuration);

    void Delete(int id);
}