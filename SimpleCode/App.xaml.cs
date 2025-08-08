using System.Configuration;
using System.Data;
using System.Windows;
using SimpleCode.Models;
using SimpleCode.Services;
using SimpleCode.Store;
using SimpleCode.ViewModels;

namespace SimpleCode;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    // Main model of the application
    private readonly EditorModel _editorModel;

    // Service for opening a folder (which is a main project of a seans)
    private readonly FolderDialogService _folderDialogService;

    // Navigation in the application 
    private readonly NavigationStore _navigationStore;

    public App()
    {
        _editorModel = new EditorModel();
        _folderDialogService = new FolderDialogService();
        _navigationStore = new NavigationStore();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _navigationStore.CurrentViewModel = new WelcomeViewModel(_editorModel, _folderDialogService, _navigationStore);
        MainWindow = new MainWindow()
        {
            DataContext = new MainViewModel(_navigationStore)
        };

        MainWindow.Show();

        base.OnStartup(e);
    }
}