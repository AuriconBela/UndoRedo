using UndoRedo.Model;

namespace UndoRedo.Repository;

public interface IDataRepository
{
    void DoCommand(DataRecord command);
    void Redo();
    void Undo();

    public bool CanUndo { get; }    
    public bool CanRedo { get; }    

    public DataRecord? Value { get; }
}