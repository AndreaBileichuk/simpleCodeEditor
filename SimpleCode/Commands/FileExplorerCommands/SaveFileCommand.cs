using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using SimpleCode.Models;
using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SimpleCode.Commands.FileExplorerCommands;

public class SaveFileCommand : CommandBase
{
    private string _extension;
    private CreateFileModalViewModel _createFileModalViewModel;

    public SaveFileCommand(CreateFileModalViewModel createFileModalViewModel, string extension)
    {
        _createFileModalViewModel = createFileModalViewModel;
        _createFileModalViewModel.PropertyChanged += OnViewModelPropertyChanged;

        _extension = extension;
    }

    public override void Execute(object? parameter)
    {
        string fullFilefPath = _createFileModalViewModel.FolderViewModel.FolderModel.FullPath + "\\"
            + _createFileModalViewModel.FileName.Trim()
            + _extension;
        if (File.Exists(fullFilefPath))
        {
            MessageBox.Show("This file already exists");
            return;
        }

        FileStream fs = File.Create(fullFilefPath);
        fs.Close();

        Window? window = parameter as Window;
        if (window != null)
        {
            window.Close();
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_createFileModalViewModel.FileName) && base.CanExecute(parameter);
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CreateFileModalViewModel.FileName))
        {
            OnCanExecuteChanged();
        }
    }
}