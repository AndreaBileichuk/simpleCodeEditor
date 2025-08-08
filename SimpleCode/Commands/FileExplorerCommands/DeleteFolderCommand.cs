using System.IO;
using SimpleCode.ViewModels;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SimpleCode.Commands.FileExplorerCommands;

public class DeleteFolderCommand : CommandBase
{
    private FolderViewModel? _parentFolder;

    public DeleteFolderCommand(FolderViewModel? parentFolder)
    {
        _parentFolder = parentFolder;
    }

    public override void Execute(object? parameter)
    {
        if (_parentFolder == null)
        {
            MessageBox.Show("Root folder can't be deleted");
            return;
        }

        FolderViewModel? folderToDelete = parameter as FolderViewModel;
        if (folderToDelete == null) return;

        string path = folderToDelete.FolderModel.FullPath;
        if (Directory.Exists(path))
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Dalbaayob");
            }
        }
    }
}