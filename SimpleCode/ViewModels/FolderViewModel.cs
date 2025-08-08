using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using SimpleCode.Commands.FileExplorerCommands;
using SimpleCode.Models;

namespace SimpleCode.ViewModels;

public class FolderViewModel : ViewModelBase
{
    private readonly FileExplorerViewModel _fileExplorerViewModel;

    private FolderModel _folderModel;

    public FolderModel FolderModel
    {
        get => _folderModel;
        set
        {
            _folderModel = value;
            OnPropertyChanged(nameof(Children));
        }
    }

    private readonly CodeWorkspaceViewModel _codeWorkspaceViewModel;
    public CodeWorkspaceViewModel CodeWorkspaceViewModel => _codeWorkspaceViewModel;

    // Commands for the Menu Context
    public ICommand CreateCppFileCommand { get; set; }
    public ICommand CreateCFileCommand { get; set; }
    public ICommand CreateHeaderFileCommand { get; set; }
    public ICommand CreateNewFolderCommand { get; set; }
    public ICommand RenameFolderCommand { get; }
    public ICommand DeleteFolderCommand { get; }
    public string Name => _folderModel.Name;
    public ObservableCollection<object> Children { get; }

    public FolderViewModel(FolderViewModel? parentFolder, FolderModel folderModel,
        CodeWorkspaceViewModel codeWorkspaceViewModel)
    {
        CreateCppFileCommand = new CreateCppFileCommand(codeWorkspaceViewModel);
        CreateCFileCommand = new CreateCFileCommand(codeWorkspaceViewModel);
        CreateHeaderFileCommand = new CreateHeaderFileCommand(codeWorkspaceViewModel);
        CreateNewFolderCommand = new CreateNewFolderCommand();
        RenameFolderCommand = new RenameFolderCommand();
        DeleteFolderCommand = new DeleteFolderCommand(parentFolder);

        _codeWorkspaceViewModel = codeWorkspaceViewModel;
        _folderModel = folderModel;

        Children = new ObservableCollection<object>(
            folderModel.SubFolders.Select(f => new FolderViewModel(this, f, codeWorkspaceViewModel))
        );

        foreach (var file in folderModel.Files)
        {
            Children.Add(new FileItemViewModel(this, file, codeWorkspaceViewModel));
        }
    }

    public FileItemViewModel AddNewFile(FileModel file)
    {
        var newFileViewModel = new FileItemViewModel(this, file, _codeWorkspaceViewModel);
        Children.Add(newFileViewModel);
        _folderModel.Files.Add(file);

        return newFileViewModel;
    }

    public void AddNewFolder(FolderModel folder)
    {
        Children.Add(new FolderViewModel(this, folder, _codeWorkspaceViewModel));
        _folderModel.SubFolders.Add(folder);
    }

    public void RenameFolder(string newName)
    {
        string oldPath = FolderModel.FullPath;
        FolderModel.RenameFolder(newName);

        foreach (var child in Children)
        {
            if (child is FolderViewModel folder)
            {
                folder.UpdatePath(oldPath, FolderModel.FullPath);
            }
            else if (child is FileItemViewModel file)
            {
                file.UpdatePath(oldPath, FolderModel.FullPath);
            }
        }

        OnPropertyChanged(nameof(Name));
    }

    public void RenameFolderSyncWithFileExplorer(string newName, string fullPath)
    {
        string oldPath = this.FolderModel.FullPath;

        this.FolderModel.Name = newName;
        this.FolderModel.FullPath = fullPath;
        foreach (var child in Children)
        {
            if (child is FolderViewModel folder)
            {
                folder.UpdatePath(oldPath, fullPath);
            }
            else if (child is FileItemViewModel file)
            {
                file.UpdatePath(oldPath, fullPath);
            }
        }

        OnPropertyChanged(nameof(Name));
    }

    public void UpdatePath(string oldParentPath, string newParentPath)
    {
        if (!this.FolderModel.FullPath.StartsWith(oldParentPath, StringComparison.OrdinalIgnoreCase))
            return;

        string relativePath = this.FolderModel.FullPath.Substring(oldParentPath.Length)
            .TrimStart(Path.DirectorySeparatorChar);
        this.FolderModel.FullPath = Path.Combine(newParentPath, relativePath);

        foreach (var child in Children ?? Enumerable.Empty<object>())
        {
            if (child is FolderViewModel folderViewModel)
                folderViewModel.UpdatePath(oldParentPath, newParentPath);
            else if (child is FileItemViewModel fileItemViewModel)
                fileItemViewModel.UpdatePath(oldParentPath, newParentPath);
        }

        OnPropertyChanged(nameof(FolderModel.FullPath));
    }

    public void RemoveFile(FileItemViewModel fileItemViewModel)
    {
        this.Children.Remove(fileItemViewModel);
        this.FolderModel.Files.Remove(fileItemViewModel.FileModel);
        _codeWorkspaceViewModel.OpenedFiles.Remove(fileItemViewModel);
        _codeWorkspaceViewModel.ActiveFileNullWithoutSave();
        _codeWorkspaceViewModel.ResetOpenedFiles();

        OnPropertyChanged(nameof(Children));
    }

    public void RemoveFolder(FolderViewModel folderViewModel)
    {
        this.Children.Remove(folderViewModel);
        this.FolderModel.SubFolders.Remove(folderViewModel.FolderModel);

        DeleteFromOpenedFilesPanel(folderViewModel);
        _codeWorkspaceViewModel.ActiveFileNullWithoutSave();
        _codeWorkspaceViewModel.ResetOpenedFiles();
    }

    private void DeleteFromOpenedFilesPanel(FolderViewModel folderViewModel)
    {
        foreach (object obj in folderViewModel.Children)
        {
            if (obj is FileItemViewModel fileItemViewModel)
            {
                if (_codeWorkspaceViewModel.OpenedFiles.Any(file => file == fileItemViewModel))
                {
                    _codeWorkspaceViewModel.OpenedFiles.Remove(fileItemViewModel);
                }
            }
            else if (obj is FolderViewModel folderViewModelObj)
            {
                DeleteFromOpenedFilesPanel(folderViewModelObj);
            }
        }
    }

    public void OnWatcherDeletedEventRaise(string fullPath)
    {
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            if (Children[i] is FolderViewModel folder)
            {
                if (folder.FolderModel.FullPath == fullPath)
                {
                    RemoveFolder(folder);
                    return;
                }

                folder.OnWatcherDeletedEventRaise(fullPath);
            }
            else if (Children[i] is FileItemViewModel file)
            {
                if (file.FileModel.FullPath == fullPath)
                {
                    RemoveFile(file);

                    return;
                }
            }
        }
    }

    public void OnWatcherRenamedEventRaise(string oldName, string fullPath)
    {
        string newName = Path.GetFileName(fullPath);
        ;
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            if (Children[i] is FolderViewModel folder)
            {
                if (folder.FolderModel.Name == oldName)
                {
                    folder.RenameFolderSyncWithFileExplorer(newName, fullPath);
                    return;
                }

                folder.OnWatcherRenamedEventRaise(Path.GetFileName(oldName), fullPath);
            }
            else if (Children[i] is FileItemViewModel file)
            {
                if (file.FileModel.Name == oldName)
                {
                    file.RenameFileSyncWithFileExplorer(newName, fullPath);
                    return;
                }
            }
        }
    }

    public void OnWatcherAddEventRaise(string fullPath)
    {
        bool isFile = Path.HasExtension(fullPath);
        string parentPath = Path.GetDirectoryName(fullPath);

        if (parentPath == this.FolderModel.FullPath)
        {
            if (isFile)
            {
                FileItemViewModel newFile = this.AddNewFile(new FileModel(fullPath));
                if (Path.GetExtension(newFile.Name) != ".exe")
                {
                    _codeWorkspaceViewModel.ActiveFile = newFile;
                }
            }
            else
            {
                AddNewFolder(new FolderModel(fullPath));
            }

            return;
        }

        for (int i = Children.Count - 1; i >= 0; i--)
        {
            if (Children[i] is FolderViewModel folder)
            {
                if (parentPath == this.FolderModel.FullPath)
                {
                    if (isFile)
                    {
                        FileItemViewModel newFile = this.AddNewFile(new FileModel(fullPath));
                        if (Path.GetExtension(newFile.Name) != ".exe")
                        {
                            _codeWorkspaceViewModel.ActiveFile = newFile;
                        }
                    }
                    else
                    {
                        folder.AddNewFolder(new FolderModel(fullPath));
                    }

                    return;
                }

                folder.OnWatcherAddEventRaise(fullPath);
            }
        }
    }
}