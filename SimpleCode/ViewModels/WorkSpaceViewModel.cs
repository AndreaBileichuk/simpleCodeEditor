using System.IO;
using SimpleCode.Models;
using SimpleCode.Store;

namespace SimpleCode.ViewModels;

public class WorkSpaceViewModel : ViewModelBase
{
    private readonly EditorModel _editorModel;
    public FileExplorerViewModel FileExplorerViewModel { get; set; }
    public CodeWorkspaceViewModel CodeWorkspaceViewModel { get; }
    public TerminalViewModel TerminalViewModel { get; set; }

    private readonly NavigationStore _navigationStore;

    public WorkSpaceViewModel(EditorModel editorModel, NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _editorModel = editorModel;
        CodeWorkspaceViewModel = new CodeWorkspaceViewModel(editorModel);
        FileExplorerViewModel = new FileExplorerViewModel(_editorModel, CodeWorkspaceViewModel, _navigationStore);
        TerminalViewModel = new TerminalViewModel(editorModel, this);
    }

    public void RefreshCurrentActiveFileContentAfterActivatingWindow()
    {
        CodeWorkspaceViewModel.FileContent =
            CodeWorkspaceViewModel.ActiveFile?.FileModel.LoadFileContentAfterActivation();
    }
}