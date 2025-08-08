using System.IO;
using System.Windows.Forms;
using SimpleCode.ViewModels;
using Exception = Mono.WebBrowser.Exception;
using MessageBox = System.Windows.MessageBox;

namespace SimpleCode.Commands.FileExplorerCommands;

public class DeleteFileCommand : CommandBase
{
    private readonly FolderViewModel _folderViewModel;
    private readonly FileItemViewModel _fileToDelete;

    public DeleteFileCommand(FolderViewModel folderViewModel, FileItemViewModel fileToDelete)
    {
        _folderViewModel = folderViewModel;
        _fileToDelete = fileToDelete;
    }

    public override void Execute(object? parameter)
    {
        string path = _fileToDelete.FileModel.FullPath;
        try
        {
            _folderViewModel.RemoveFile(_fileToDelete);
            _fileToDelete.CloseFileCommand.Execute(_fileToDelete);
            File.Delete(path);
        }
        catch (Exception e)
        {
            MessageBox.Show("File was not delete");
        }
    }
}