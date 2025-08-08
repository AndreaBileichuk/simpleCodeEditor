using System.Windows;
using SimpleCode.Models;
using SimpleCode.Services;
using SimpleCode.Store;
using SimpleCode.ViewModels;

namespace SimpleCode.Commands;

public class OpenFolderCommand : CommandBase
{
    private readonly EditorModel _editorModel;
    private readonly FolderDialogService _folderDialogService;
    private readonly NavigationStore _navigationStore;

    public OpenFolderCommand(EditorModel editorModel, FolderDialogService folderDialogService,
        NavigationStore navigationStore)
    {
        _editorModel = editorModel;
        _folderDialogService = folderDialogService;
        _navigationStore = navigationStore;
    }

    public override void Execute(object? parameter)
    {
        string path = _folderDialogService.OpenFolderDialog();
        if (string.IsNullOrEmpty(path))
        {
            MessageBox.Show("Something went wrong!");
            return;
        }

        _editorModel.OpenProject(path);

        _navigationStore.CurrentViewModel = new WorkSpaceViewModel(_editorModel, _navigationStore);
    }
}