using System.Windows.Input;

namespace UndoRedo.ViewModel;

public interface ICommandExtended : ICommand
{
    public void OnCanExecuteChanged();
}
