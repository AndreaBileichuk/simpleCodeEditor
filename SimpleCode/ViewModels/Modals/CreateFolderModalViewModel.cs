using System.Windows.Input;
using SimpleCode.Commands.FileExplorerCommands;

namespace SimpleCode.ViewModels.Modals;

public class CreateFolderModalViewModel : ViewModelBase
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

    public CreateFolderModalViewModel(FolderViewModel folderViewModel)
    {
        FolderViewModel = folderViewModel;

        SaveCommand = new SaveFolderCommand(this);
        CancelCommand = new CancelCommand();
    }
}