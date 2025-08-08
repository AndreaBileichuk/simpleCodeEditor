using System.Windows;

namespace SimpleCode.Commands;

public class CloseProgramCommand : CommandBase
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