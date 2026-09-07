using DataQualityPlatform.Persistence.Entities;

namespace DataQualityPlatform.Repositories;

/// <summary>
/// Stores and retrieves processing execution history.
/// </summary>
public interface IProcessingRunRepository
{
    void Add(ProcessingRunEntity run);

    ProcessingRunEntity? GetById(int id);

    List<ProcessingRunEntity> GetAll();

    void Update(ProcessingRunEntity run);

    void Delete(int id);
}
