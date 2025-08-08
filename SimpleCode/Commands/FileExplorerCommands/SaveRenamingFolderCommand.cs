using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;

namespace SimpleCode.Commands.FileExplorerCommands;

public class SaveRenamingFolderCommand : CommandBase
{
    private readonly RenameFolderModalViewModel _modalViewModel;

    public SaveRenamingFolderCommand(RenameFolderModalViewModel modalViewModel)
    {
        _modalViewModel = modalViewModel;

        _modalViewModel.PropertyChanged += ViewModelPropertyChanged;
    }

    public override void Execute(object? parameter)
    {
        _modalViewModel.FolderViewModel.RenameFolder(_modalViewModel.FolderName);

        Window window = parameter as Window;
        if (window != null) window.Close();
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_modalViewModel.FolderName) && base.CanExecute(parameter);
    }

    private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(RenameFolderModalViewModel.FolderName))
        {
            OnCanExecuteChanged();
        }

        ;
    }
}