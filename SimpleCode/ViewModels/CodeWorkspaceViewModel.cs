using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using SimpleCode.Models;
using System.Windows.Input;
using SimpleCode.Commands;
using SimpleCode.Commands.FileExplorerCommands;

namespace SimpleCode.ViewModels;

public class CodeWorkspaceViewModel : ViewModelBase
{
    // Commands
    public ICommand SaveFileCodeCommand { get; }
    public ICommand TabKeyPressedCommand { get; }

    private EditorModel _editorModel;

    public EditorModel EditorModel
    {
        get => _editorModel;
        set { _editorModel = value; }
    }

    private ObservableCollection<FileItemViewModel> _openedFiles;

    public ObservableCollection<FileItemViewModel> OpenedFiles
    {
        get => _openedFiles;
    }

    private ObservableCollection<int> _lines;

    public ObservableCollection<int> Lines
    {
        get => _lines;
        set
        {
            _lines = value;
            OnPropertyChanged(nameof(Lines));
        }
    }

    public string? FileContent
    {
        get => ActiveFile?.FileModel?.Content ?? "";
        set
        {
            if (ActiveFile?.FileModel == null) return;

            ActiveFile.FileModel.IsModified = true;
            ActiveFile.FileModel.Content = value;
            UpdateLineNumbers();
            OnPropertyChanged(nameof(FileContent));
        }
    }

    private FileItemViewModel? _activeFile;

    public FileItemViewModel? ActiveFile
    {
        get => _activeFile;
        set
        {
            if (_activeFile == value) return;

            if (value == null)
            {
                _activeFile = null;
                return;
            }

            ;

            if (_activeFile != null)
                _activeFile.IsActive = false;

            if (_activeFile?.FileModel != null && _activeFile.FileModel.IsModified)
            {
                _activeFile.FileModel.SaveChanges();
            }

            _activeFile = value;
            if (_activeFile == null || !File.Exists(_activeFile.FileModel.FullPath)) return;

            _activeFile.IsActive = true;
            _editorModel.ActiveFile = value?.FileModel;

            if (value != null && value?.FileModel != null)
            {
                FileContent = value.FileModel.LoadFileContent();
            }

            if (!_openedFiles.Any(f => f.FileModel == value.FileModel))
            {
                _openedFiles.Add(value);
                OnPropertyChanged(nameof(OpenedFiles));
            }

            UpdateLineNumbers();
            OnPropertyChanged(nameof(ActiveFile));
            OnPropertyChanged(nameof(FileContent));
        }
    }

    public CodeWorkspaceViewModel(EditorModel editorModel)
    {
        _editorModel = editorModel;
        _lines = new ObservableCollection<int>();
        _openedFiles = new ObservableCollection<FileItemViewModel>();
        SaveFileCodeCommand = new SaveFileCodeCommand();
        TabKeyPressedCommand = new TabKeyPressedCommand();

        UpdateLineNumbers();
    }

    private void UpdateLineNumbers()
    {
        if (FileContent == null) return;
        int lineCount = FileContent.Split('\n').Length;
        Lines.Clear();
        for (int i = 1; i <= lineCount; i++)
        {
            Lines.Add(i);
        }
    }

    public void ActiveFileNullWithoutSave()
    {
        _activeFile = null;
    }

    public void ResetOpenedFiles()
    {
        var count = this.OpenedFiles.Count;
        if (count != 0)
        {
            this.ActiveFile = this.OpenedFiles[count - 1];
        }
        else
        {
            this.FileContent = "";
            this.ActiveFile = null;
            _lines = new ObservableCollection<int>() { 1 };
            OnPropertyChanged(FileContent);
        }
    }
}