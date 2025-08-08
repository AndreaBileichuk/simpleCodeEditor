using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;

namespace SimpleCode.Commands.FileExplorerCommands;

public class SaveRenamingFileCommand : CommandBase
{
    private readonly RenameFileModalViewModel _modalViewModel;

    public SaveRenamingFileCommand(RenameFileModalViewModel modalViewModel)
    {
        _modalViewModel = modalViewModel;
        _modalViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    public override void Execute(object? parameter)
    {
        _modalViewModel.FileItemViewModel.RenameFile(_modalViewModel.FileName);

        Window window = parameter as Window;
        if (window != null) window.Close();
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_modalViewModel.FileName) && base.CanExecute(parameter);
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(RenameFileModalViewModel.FileName))
        {
            OnCanExecuteChanged();
        }
    }
}