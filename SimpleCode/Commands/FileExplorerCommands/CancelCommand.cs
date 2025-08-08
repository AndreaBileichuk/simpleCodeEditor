using System.Windows;

namespace SimpleCode.Commands.FileExplorerCommands;

public class CancelCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        var window = parameter as Window;
        if (window != null)
        {
            window.Close();
        }
    }
}