using System.Windows.Forms;
using SimpleCode.Models;
using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;

namespace SimpleCode.Commands.FileExplorerCommands;

public class CreateCppFileCommand : CommandBase
{
    private readonly CodeWorkspaceViewModel _codeWorkspaceViewModel;

    public CreateCppFileCommand(CodeWorkspaceViewModel codeWorkspaceViewModel)
    {
        _codeWorkspaceViewModel = codeWorkspaceViewModel;
    }

    public override void Execute(object? parameter)
    {
        CreateFileModalView modalWindow = new CreateFileModalView()
        {
            DataContext = new CreateFileModalViewModel(_codeWorkspaceViewModel, ".cpp", parameter as FolderViewModel)
        };

        modalWindow.ShowDialog();
    }
}