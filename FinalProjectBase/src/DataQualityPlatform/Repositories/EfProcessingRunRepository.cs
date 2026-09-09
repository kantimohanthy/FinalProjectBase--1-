using DataQualityPlatform.Persistence;
using DataQualityPlatform.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

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
        _context.ProcessingRuns.Add(run);
        _context.SaveChanges();
    }

    public ProcessingRunEntity? GetById(int id)
    {
        return _context.ProcessingRuns
            .Include(run => run.QualityIssues)
            .SingleOrDefault(run => run.Id == id);
    }

    public List<ProcessingRunEntity> GetAll()
    {
        return _context.ProcessingRuns
            .Include(run => run.QualityIssues)
            .ToList();
    }

    public void Update(ProcessingRunEntity run)
    {
        _context.ProcessingRuns.Update(run);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        ProcessingRunEntity? run = _context.ProcessingRuns.Find(id);

        if (run is null)
        {
            return;
        }

        _context.ProcessingRuns.Remove(run);
        _context.SaveChanges();
    }
}