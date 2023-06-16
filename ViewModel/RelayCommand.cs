using System;

namespace UndoRedo.ViewModel;

public class RelayCommand<T> : ICommandExtended
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool> _canExecute;


    /// Wires CanExecuteChanged event 
    /// </summary>
    public event EventHandler? CanExecuteChanged;
    public RelayCommand(Action<T> execute) : this(execute, null!) { }

    /// <summary>
    /// Creates instance of the command handler
    /// </summary>
    /// <param name="execute">Action to be executed by the command</param>
    /// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
    public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Forces checking if execute is allowed
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute((T)parameter);
    public void Execute(object? parameter) => _execute((T)parameter);
    public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
