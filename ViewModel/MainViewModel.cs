using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UndoRedo.Misc;
using UndoRedo.Model;
using UndoRedo.Repository;

namespace UndoRedo.ViewModel;

public class MainViewModel : IMainViewModel
{
    private readonly IDataRepository _repository;

    private readonly ICommandExtended _addCommand;

    private readonly ICommandExtended _undoCommand;

    private readonly ICommandExtended _redoCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    private static readonly PropertyChangedEventArgs _emptyChangeArgs = new(string.Empty);
    private static readonly IDictionary<string, PropertyChangedEventArgs> _changedProperties = new Dictionary<string, PropertyChangedEventArgs>();

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

    public ICommandExtended AddCommand => _addCommand;
    public ICommandExtended UndoCommand => _undoCommand;
    public ICommandExtended RedoCommand => _redoCommand;

    public DataRecord? Value => _repository.Value;
    private void AddAction(string value)
    {
        _repository.DoCommand(new DataRecord(value, DateTime.Now));
        Changed();
    }

    private void UndoAction(DataRecord record)
    {
        _repository.Undo();
        Changed();
    }

    private void RedoAction(DataRecord record)
    {
        _repository.Redo();
        Changed();
    }

    private void Changed()
    {
        _addCommand.OnCanExecuteChanged();
        _undoCommand.OnCanExecuteChanged();
        _redoCommand.OnCanExecuteChanged();
        OnPropertyChanged(nameof(Value));
    }

    private bool CanExecuteForRedo(DataRecord record)
    {
        return _repository.CanRedo;
    }

    private bool CanExecuteForUndo(DataRecord record)
    {
        return _repository.CanUndo;
    }


    #region "Boilerplate stuff"
    protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
    {
        OnPropertyChanged(ExpressionHelper.Name(expression));
    }

    protected virtual void OnPropertyChanged()
    {
        OnPropertyChanged(null!);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;
        if (handler != null)
        {
            if (propertyName == null)
            {
                handler(this, _emptyChangeArgs);
            }
            else
            {
                PropertyChangedEventArgs args;
                if (!_changedProperties.TryGetValue(propertyName, out args!))
                {
                    args = new PropertyChangedEventArgs(propertyName);
                    _changedProperties.Add(propertyName, args);
                }

                handler(this, args);
            }
        }
    }

    protected virtual bool SetPropertyAndNotify<T>(ref T existingValue, T newValue, Expression<Func<T>> expression)
    {
        if (EqualityComparer<T>.Default.Equals(existingValue, newValue))
        {
            return false;
        }

        existingValue = newValue;
        OnPropertyChanged(expression);

        return true;
    }
    #endregion
}
