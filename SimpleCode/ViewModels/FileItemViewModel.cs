using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using SimpleCode.Commands;
using SimpleCode.Commands.FileExplorerCommands;
using SimpleCode.Models;

namespace SimpleCode.ViewModels;

public class FileItemViewModel : ViewModelBase
{
    private readonly CodeWorkspaceViewModel _codeWorkspaceViewModel;
    public ICommand OpenFileCommand { get; }
    public ICommand RenameFileCommand { get; }
    public ICommand CloseFileCommand { get; }
    public ICommand DeleteFileCommand { get; }
    public FileModel FileModel { get; set; }
    public string Name => FileModel?.Name;
    public string Extension => Path.GetExtension(Name);

    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (value == _isActive) return;
            _isActive = value;
            OnPropertyChanged(nameof(IsActive));
        }
    }

    public FileItemViewModel(FolderViewModel? parentFolder, FileModel fileModel,
        CodeWorkspaceViewModel codeWorkspaceViewModel)
    {
        _codeWorkspaceViewModel = codeWorkspaceViewModel;

        FileModel = fileModel;
        OpenFileCommand = new OpenFileCommand(this, codeWorkspaceViewModel);
        RenameFileCommand = new RenameFileCommand();
        CloseFileCommand = new CloseFileCommand(this, codeWorkspaceViewModel);
        DeleteFileCommand = new DeleteFileCommand(parentFolder, this);
    }

    public void RenameFile(string newName)
    {
        FileModel.RenameFile(newName);
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Extension));
    }

    public void RenameFileSyncWithFileExplorer(string newName, string fullPath)
    {
        FileModel.Name = newName;
        FileModel.FullPath = fullPath;
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Extension));
    }

    public void UpdatePath(string oldParentPath, string newParentPath)
    {
        this.FileModel.FullPath = this.FileModel.FullPath.Replace(oldParentPath, newParentPath);
        OnPropertyChanged(nameof(FileModel.FullPath));
    }
}