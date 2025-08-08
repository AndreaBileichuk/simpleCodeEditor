using System.Reflection.Metadata;
using System.Windows.Forms;
using SimpleCode.Models;
using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;

namespace SimpleCode.Commands.FileExplorerCommands;

public class CreateNewFolderCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        CreateFolderModalView modalWindow = new CreateFolderModalView()
        {
            DataContext = new CreateFolderModalViewModel(parameter as FolderViewModel)
        };

        modalWindow.ShowDialog();
    }
}