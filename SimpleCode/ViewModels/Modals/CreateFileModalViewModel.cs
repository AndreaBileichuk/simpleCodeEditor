using System.Windows.Input;
using System.Xml.XPath;
using SimpleCode.Commands.FileExplorerCommands;
using SimpleCode.Models;

namespace SimpleCode.ViewModels.Modals;

public class CreateFileModalViewModel : ViewModelBase
{
    public CodeWorkspaceViewModel CodeWorkspaceViewModel;
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
    public string Extension { get; }
    public FolderViewModel FolderViewModel { get; }

    public CreateFileModalViewModel(CodeWorkspaceViewModel codeWorkspaceViewModel, string extension,
        FolderViewModel folderViewModel)
    {
        CodeWorkspaceViewModel = codeWorkspaceViewModel;
        Extension = "Extension: " + extension;
        FolderViewModel = folderViewModel;

        SaveCommand = new SaveFileCommand(this, extension);
        CancelCommand = new CancelCommand();
    }
}