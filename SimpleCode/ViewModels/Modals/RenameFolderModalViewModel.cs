using System.Windows.Input;
using SimpleCode.Commands.FileExplorerCommands;

namespace SimpleCode.ViewModels.Modals;

public class RenameFolderModalViewModel : ViewModelBase
{
    private string _folderName;

    public string FolderName
    {
        get => _folderName;
        set
        {
            _folderName = value;
            OnPropertyChanged(nameof(FolderName));
        }
    }

    public ICommand SaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public FolderViewModel FolderViewModel { get; }

    public RenameFolderModalViewModel(FolderViewModel folder)
    {
        FolderViewModel = folder;
        FolderName = FolderViewModel.Name;

        SaveCommand = new SaveRenamingFolderCommand(this);
        CancelCommand = new CancelCommand();
    }
}