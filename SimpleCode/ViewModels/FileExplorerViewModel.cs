using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using SimpleCode.Models;
using SimpleCode.Store;

namespace SimpleCode.ViewModels;

public class FileExplorerViewModel : ViewModelBase
{
    public ObservableCollection<FolderViewModel>? RootFolders { get; set; }
    private EditorModel _editorModel;
    private readonly NavigationStore _navigationStore;
    private FileSystemWatcher _watcher;

    public FileExplorerViewModel(EditorModel editorModel, CodeWorkspaceViewModel codeWorkspaceViewModel,
        NavigationStore navigationStore)
    {
        _editorModel = editorModel;
        _navigationStore = navigationStore;

        RootFolders = new ObservableCollection<FolderViewModel>()
        {
            new FolderViewModel(null, editorModel.RootFolder, codeWorkspaceViewModel)
        };

        InitializeWatcher();
    }

    private void InitializeWatcher()
    {
        _watcher = new FileSystemWatcher
        {
            Path = _editorModel.RootFolder.FullPath,
            NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite,
            Filter = "*.*",
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _watcher.Deleted += (s, e) =>
            Application.Current.Dispatcher.BeginInvoke(() => RootFolders?[0].OnWatcherDeletedEventRaise(e.FullPath));
        _watcher.Created += (s, e) =>
            Application.Current.Dispatcher.Invoke(() => RootFolders?[0].OnWatcherAddEventRaise(e.FullPath));
        _watcher.Renamed += (s, e) =>
            Application.Current.Dispatcher.Invoke(() =>
                RootFolders?[0].OnWatcherRenamedEventRaise(e.OldName, e.FullPath));
    }
}