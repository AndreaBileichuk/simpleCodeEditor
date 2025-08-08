using System.ComponentModel;
using System.IO;
using System.Windows;
using SimpleCode.Models;
using SimpleCode.Services;
using SimpleCode.Store;
using SimpleCode.ViewModels;
using SimpleCode.ViewModels.Modals;
using SimpleCode.Views.Modals;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SimpleCode.Commands;

public class CreateRootFolder : CommandBase
{
    private readonly EditorModel _editorModel;
    private readonly NavigationStore _navigationStore;
    private FolderDialogService _folderDialogService;


    public CreateRootFolder(FolderDialogService folderDialogService, EditorModel editorModel,
        NavigationStore navigationStore)
    {
        _folderDialogService = folderDialogService;
        _editorModel = editorModel;
        _navigationStore = navigationStore;
    }

    public override void Execute(object? parameter)
    {
        string path = _folderDialogService.OpenFolderDialog();
        if (string.IsNullOrEmpty(path))
        {
            MessageBox.Show("You didn't choose the path, try again please!");
            return;
        }

        var viewModel = new CreateFolderModalViewModel(null);
        viewModel.SaveCommand = new SaveRootFolderCreation(path, _editorModel, _navigationStore, viewModel);

        CreateFolderModalView view = new CreateFolderModalView()
        {
            DataContext = viewModel
        };

        view.TitleBlock.Text = path;
        view.Width = 1000;
        view.ShowDialog();
    }
}

public class SaveRootFolderCreation : CommandBase
{
    private readonly EditorModel _editorModel;
    private readonly NavigationStore _navigationStore;
    private readonly CreateFolderModalViewModel _viewModel;
    private readonly string _path;

    public SaveRootFolderCreation(string path, EditorModel editorModel, NavigationStore navigationStore,
        CreateFolderModalViewModel viewModel)
    {
        _path = path;
        _viewModel = viewModel;
        _editorModel = editorModel;
        _navigationStore = navigationStore;

        _viewModel.PropertyChanged += ViewModelPropertyChanged;
    }

    public override void Execute(object? parameter)
    {
        string projectPath = _path + "\\" + _viewModel.FolderName;
        if (Directory.Exists(projectPath))
        {
            MessageBox.Show("This folder already exists");
            _viewModel.FolderName = "";
            return;
        }

        DirectoryInfo di = Directory.CreateDirectory(projectPath);

        _editorModel.OpenProject(projectPath);
        _navigationStore.CurrentViewModel = new WorkSpaceViewModel(_editorModel, _navigationStore);

        Window? window = parameter as Window;
        if (window != null) window.Close();
    }

    public override bool CanExecute(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(_viewModel.FolderName) && base.CanExecute(parameter);
    }

    protected void ViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CreateFolderModalViewModel.FolderName))
        {
            OnCanExecuteChanged();
        }
    }
}