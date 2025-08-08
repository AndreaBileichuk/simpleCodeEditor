using System.ComponentModel;
using System.IO;
using System.Windows;
using SimpleCode.Models;
using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SimpleCode.Commands.FileExplorerCommands;

public class SaveFolderCommand : CommandBase
{
    private CreateFolderModalViewModel _createFolderModalViewModel;

    public SaveFolderCommand(CreateFolderModalViewModel createFolderModalViewModel)
    {
        _createFolderModalViewModel = createFolderModalViewModel;
        _createFolderModalViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    public override void Execute(object? parameter)
    {
        string path = _createFolderModalViewModel.FolderViewModel.FolderModel.FullPath + "\\"
            + _createFolderModalViewModel.FolderName.Trim();

        if (Directory.Exists(path))
        {
            MessageBox.Show("That path exists already.");
            return;
        }

        DirectoryInfo di = Directory.CreateDirectory(path);

        Window? window = parameter as Window;
        if (window != null)
        {
            window.Close();
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_createFolderModalViewModel?.FolderName) && base.CanExecute(parameter);
        ;
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CreateFolderModalViewModel.FolderName))
        {
            OnCanExecuteChanged();
        }
    }
}