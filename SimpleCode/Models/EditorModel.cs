using System.IO;
using System.Windows.Forms;

namespace SimpleCode.Models;

public class EditorModel
{
    public FolderModel RootFolder { get; set; }
    public FileModel? ActiveFile { get; set; }
    public bool IsProjectLoaded { get; set; } = false;

    public void OpenProject(string path)
    {
        if (!Directory.Exists(path)) return;
        IsProjectLoaded = true;

        RootFolder = new FolderModel(path);
        RootFolder.LoadFoldersAndFiles();
    }

    public void GetAllNewItems()
    {
        RootFolder?.GetAllNewItems();
    }
}