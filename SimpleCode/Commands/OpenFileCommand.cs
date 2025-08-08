using System.Windows.Forms;
using SimpleCode.ViewModels;

namespace SimpleCode.Commands;

public class OpenFileCommand : CommandBase
{
    private readonly FileItemViewModel _file;
    private readonly CodeWorkspaceViewModel _codeWorkspaceViewModel;

    public OpenFileCommand(FileItemViewModel file, CodeWorkspaceViewModel codeWorkspaceViewModel)
    {
        _file = file;
        _codeWorkspaceViewModel = codeWorkspaceViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (_file.Extension == ".exe")
        {
            MessageBox.Show("Cannot open executable files");
            return;
        }

        _codeWorkspaceViewModel.ActiveFile = _file;
    }
}