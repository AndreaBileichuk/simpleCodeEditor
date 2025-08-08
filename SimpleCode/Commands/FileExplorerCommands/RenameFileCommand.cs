using System.Net;
using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;

namespace SimpleCode.Commands.FileExplorerCommands;

public class RenameFileCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        CreateFileModalView modalWindow = new CreateFileModalView()
        {
            DataContext = new RenameFileModalViewModel(parameter as FileItemViewModel)
        };

        modalWindow.TitleBlock.Text = "Rename File";
        modalWindow.ShowDialog();
    }
}