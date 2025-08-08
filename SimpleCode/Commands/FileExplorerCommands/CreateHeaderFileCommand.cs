using SimpleCode.Models;
using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;

namespace SimpleCode.Commands.FileExplorerCommands;

public class CreateHeaderFileCommand : CommandBase
{
    private readonly CodeWorkspaceViewModel _codeWorkspaceViewModel;

    public CreateHeaderFileCommand(CodeWorkspaceViewModel codeWorkspaceViewModel)
    {
        _codeWorkspaceViewModel = codeWorkspaceViewModel;
    }

    public override void Execute(object? parameter)
    {
        CreateFileModalView modalWindow = new CreateFileModalView()
        {
            DataContext = new CreateFileModalViewModel(_codeWorkspaceViewModel, ".h", parameter as FolderViewModel)
        };

        modalWindow.ShowDialog();
    }
}