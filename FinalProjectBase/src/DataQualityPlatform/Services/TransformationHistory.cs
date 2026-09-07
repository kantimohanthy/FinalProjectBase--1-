using DataQualityPlatform.Models;

namespace DataQualityPlatform.Services;

public class TransformationHistory
{
    private readonly Stack<Dataset> _history = new();

    public bool CanUndo => _history.Count > 0;

    public void Save(Dataset dataset)
    {
        // TODO(STUDENT): Save a snapshot before applying a transformation.
        // TODO(STUDENT): Use the Stack<Dataset> to support last-in, first-out undo.
        throw new NotImplementedException("TODO: Student implementation.");
    }

    public Dataset Undo()
    {
        // TODO(STUDENT): Return the latest saved Dataset snapshot.
        // TODO(STUDENT): Throw an appropriate exception when there is no snapshot to undo.
        throw new NotImplementedException("TODO: Student implementation.");
    }
}
