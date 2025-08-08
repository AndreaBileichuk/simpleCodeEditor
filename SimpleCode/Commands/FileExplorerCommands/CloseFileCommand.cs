using System.Windows.Input;
using SimpleCode.ViewModels;

namespace SimpleCode.Commands.FileExplorerCommands;

public class CloseFileCommand : CommandBase
{
    private readonly FileItemViewModel _fileItemViewModel;
    private readonly CodeWorkspaceViewModel _codeWorkspaceViewModel;

    public CloseFileCommand(FileItemViewModel fileItemViewModel, CodeWorkspaceViewModel codeWorkspaceViewModel)
    {
        _fileItemViewModel = fileItemViewModel;
        _codeWorkspaceViewModel = codeWorkspaceViewModel;
    }

    public override void Execute(object? parameter)
    {
        _codeWorkspaceViewModel.OpenedFiles.Remove(_fileItemViewModel);
        _codeWorkspaceViewModel.ResetOpenedFiles();
    }
}