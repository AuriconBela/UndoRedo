using System;
using UndoRedo.Misc;
using UndoRedo.Model;
using UndoRedo.Repository;

namespace UndoRedo.ViewModel;

public class MainViewModel : BaseViewModel, IMainViewModel
{
    private readonly IDataRepository? _repository;

    private readonly ICommandExtended? _addCommand;

    private readonly ICommandExtended? _undoCommand;

    private readonly ICommandExtended? _redoCommand;

    public MainViewModel()
    {

    }

    public MainViewModel(IDataRepository dataRepository)
    {
        _repository = dataRepository;
        _addCommand = new RelayCommand<string>(AddAction);
        _undoCommand = new RelayCommand<DataRecord>(UndoAction, CanExecuteForUndo);
        _redoCommand = new RelayCommand<DataRecord>(RedoAction, CanExecuteForRedo);
    }

    public static string TitleString => Constants.Title;
    public static string UndoString => Constants.Undo;
    public static string RedoString => Constants.Redo;
    public static string SaveString => Constants.Save;

    public ICommandExtended AddCommand => _addCommand!;
    public ICommandExtended UndoCommand => _undoCommand!;
    public ICommandExtended RedoCommand => _redoCommand!;

    public DataRecord? Value => _repository!.Value;
    private void AddAction(string value)
    {
        _repository?.DoCommand(new DataRecord(value, DateTime.Now));
        Changed();
    }

    private void UndoAction(DataRecord record)
    {
        _repository?.Undo();
        Changed();
    }

    private void RedoAction(DataRecord record)
    {
        _repository?.Redo();
        Changed();
    }

    private void Changed()
    {
        _undoCommand?.OnCanExecuteChanged();
        _redoCommand?.OnCanExecuteChanged();
        OnPropertyChanged(nameof(Value));
    }

    private bool CanExecuteForRedo(DataRecord record)
    {
        return _repository?.CanRedo ?? false;
    }

    private bool CanExecuteForUndo(DataRecord record)
    {
        return _repository?.CanUndo ?? false;
    }
}
