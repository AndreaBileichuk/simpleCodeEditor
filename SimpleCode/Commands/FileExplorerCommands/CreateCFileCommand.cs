using SimpleCode.Models;
using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;

namespace SimpleCode.Commands.FileExplorerCommands;

public class CreateCFileCommand : CommandBase
{
    private readonly CodeWorkspaceViewModel _codeWorkspaceViewModel;

    public CreateCFileCommand(CodeWorkspaceViewModel codeWorkspaceViewModel)
    {
        _codeWorkspaceViewModel = codeWorkspaceViewModel;
    }

    public override void Execute(object? parameter)
    {
        CreateFileModalView modalWindow = new CreateFileModalView()
        {
            DataContext = new CreateFileModalViewModel(_codeWorkspaceViewModel, ".c", parameter as FolderViewModel)
        };

        modalWindow.ShowDialog();
    }
}