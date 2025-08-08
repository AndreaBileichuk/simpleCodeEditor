using System.ComponentModel;
using SimpleCode.ViewModels;

namespace SimpleCode.Commands;

class ExecuteCommand : CommandBase
{
    private readonly Action _function;
    private readonly TerminalViewModel _terminalViewModel;

    public ExecuteCommand(Action function, TerminalViewModel terminalViewModel)
    {
        _function = function;
        _terminalViewModel = terminalViewModel;

        _terminalViewModel.PropertyChanged += ViewModelPropertyChanged;
    }

    public override void Execute(object? parameter)
    {
        _function?.Invoke();
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_terminalViewModel.InputCommand) && base.CanExecute(parameter);
    }

    private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TerminalViewModel.InputCommand))
        {
            OnCanExecuteChanged();
        }
    }
}