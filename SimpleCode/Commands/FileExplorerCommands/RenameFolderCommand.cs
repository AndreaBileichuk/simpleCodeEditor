using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;

namespace SimpleCode.Commands.FileExplorerCommands;

public class RenameFolderCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        CreateFolderModalView renameFolderDialogView = new CreateFolderModalView()
        {
            DataContext = new RenameFolderModalViewModel(parameter as FolderViewModel)
        };

        renameFolderDialogView.TitleBlock.Text = "Rename Folder";
        renameFolderDialogView.ShowDialog();
    }
}