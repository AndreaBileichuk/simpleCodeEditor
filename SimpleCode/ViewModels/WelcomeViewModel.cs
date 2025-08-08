using System.CodeDom;
using System.Windows.Input;
using SimpleCode.Commands;
using SimpleCode.Models;
using SimpleCode.Services;
using SimpleCode.Store;

namespace SimpleCode.ViewModels;

public class WelcomeViewModel : ViewModelBase
{
    public ICommand OpenFolderCommand { get; }
    public ICommand CreateFolderCommand { get; }
    public ICommand CloseProgramCommand { get; }

    public WelcomeViewModel(EditorModel editorModel, FolderDialogService folderDialogService,
        NavigationStore navigationStore)
    {
        OpenFolderCommand = new OpenFolderCommand(editorModel, folderDialogService, navigationStore);
        //CreateFolderCommand = new CreateFolderCommand();
        CreateFolderCommand = new CreateRootFolder(folderDialogService, editorModel, navigationStore);
        CloseProgramCommand = new CloseProgramCommand();
    }
}