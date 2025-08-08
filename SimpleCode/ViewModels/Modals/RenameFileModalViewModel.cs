using System.Windows.Input;
using SimpleCode.Commands.FileExplorerCommands;

namespace SimpleCode.ViewModels.Modals;

public class RenameFileModalViewModel : ViewModelBase
{
    private string _fileName;

    public string FileName
    {
        get => _fileName;
        set
        {
            _fileName = value;
            OnPropertyChanged(nameof(FileName));
        }
    }

    public ICommand SaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public FileItemViewModel FileItemViewModel { get; }

    public RenameFileModalViewModel(FileItemViewModel fileViewModal)
    {
        FileItemViewModel = fileViewModal;
        FileName = fileViewModal.Name;

        SaveCommand = new SaveRenamingFileCommand(this);
        CancelCommand = new CancelCommand();
    }
}