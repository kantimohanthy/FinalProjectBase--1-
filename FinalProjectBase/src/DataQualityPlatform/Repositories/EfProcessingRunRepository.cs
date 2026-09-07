using DataQualityPlatform.Persistence;
using DataQualityPlatform.Persistence.Entities;

namespace DataQualityPlatform.Repositories;

public class EfProcessingRunRepository : IProcessingRunRepository
{
    private readonly DataQualityContext _context;

    public EfProcessingRunRepository(DataQualityContext context)
    {
        _context = context;
    }

    public void Add(ProcessingRunEntity run)
    {
        // TODO(STUDENT): Add the run to the DbContext and save changes.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    public ProcessingRunEntity? GetById(int id)
    {
        // TODO(STUDENT): Retrieve one processing run by primary key.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    public List<ProcessingRunEntity> GetAll()
    {
        // TODO(STUDENT): Return all processing runs, including related quality issues when useful.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    public void Update(ProcessingRunEntity run)
    {
        // TODO(STUDENT): Update an existing processing run and save changes.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    public void Delete(int id)
    {
        // TODO(STUDENT): Delete the processing run with the specified id and save changes.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
