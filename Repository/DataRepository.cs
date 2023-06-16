using System.Collections.Generic;
using System.Linq;
using UndoRedo.Model;

namespace UndoRedo.Repository;

public class DataRepository : IDataRepository
{
    private readonly Stack<DataRecord?> _undoList = new();
    private readonly Stack<DataRecord?> _redoList = new();

    public void DoCommand(DataRecord command)
    {
        _undoList.Push(command);
    }
    public void Redo()
    {
        var command = _redoList.Pop();
        _undoList.Push(command);
    }
    public void Undo()
    {
        var command = _undoList.Pop();
        _redoList.Push(command);
    }

    public bool CanUndo => _undoList.Any();

    public bool CanRedo => _redoList.Any();

    public DataRecord? Value => _undoList.Any() ? _undoList.Peek() : null;
}
