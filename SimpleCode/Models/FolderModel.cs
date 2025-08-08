using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace SimpleCode.Models;

public class FolderModel
{
    public string Name { get; set; }
    public string FullPath { get; set; }
    public ObservableCollection<FileModel> Files { get; set; }
    public ObservableCollection<FolderModel> SubFolders { get; set; }

    public FolderModel(string fullPath)
    {
        FullPath = fullPath;
        Name = Path.GetFileName(fullPath);
        Files = new ObservableCollection<FileModel>();
        SubFolders = new ObservableCollection<FolderModel>();
    }

    public void LoadFoldersAndFiles()
    {
        GetAllFiles();
        GetAllSubfolders();
    }

    public void RenameFolder(string newName)
    {
        string directory = Path.GetDirectoryName(FullPath);
        string newFullPath = Path.Combine(directory, newName);

        if (Directory.Exists(FullPath))
        {
            if (Directory.Exists(newFullPath))
            {
                MessageBox.Show("A folder with the new name already exists.");
                return;
            }

            try
            {
                Directory.Move(FullPath, newFullPath);
                Name = newName;
                FullPath = newFullPath;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("You don't have permission to rename this folder.");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"An error occurred while renaming the folder: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("The folder to rename does not exist.");
        }
    }

    private void GetAllSubfolders()
    {
        var subdirectoryEntries = Directory.GetDirectories(FullPath, "*", SearchOption.TopDirectoryOnly);

        foreach (var dir in subdirectoryEntries)
        {
            FolderModel subFolderModel = new FolderModel(dir);
            subFolderModel.LoadFoldersAndFiles();
            SubFolders.Add(subFolderModel);
        }
    }

    private void GetAllFiles()
    {
        var fileEntries = Directory.GetFiles(FullPath);

        foreach (var filePath in fileEntries)
        {
            Files.Add(new FileModel(filePath));
        }
    }

    public void GetAllNewItems()
    {
        CheckForNewFiles(this);
        CheckForNewFolders(this);
    }

    private void CheckForNewFiles(FolderModel folder)
    {
        var fileEntries = Directory.GetFiles(folder.FullPath);

        foreach (var filePath in fileEntries)
        {
            if (!folder.Files.Any(f => f.FullPath == filePath))
            {
                folder.Files.Add(new FileModel(filePath));
            }
        }

        foreach (var subFolder in folder.SubFolders)
        {
            CheckForNewFiles(subFolder);
        }
    }

    private void CheckForNewFolders(FolderModel folder)
    {
        var subdirectoryEntries = Directory.GetDirectories(folder.FullPath);

        foreach (var dir in subdirectoryEntries)
        {
            if (!folder.SubFolders.Any(f => f.FullPath == dir))
            {
                var newSubFolder = new FolderModel(dir);
                newSubFolder.LoadFoldersAndFiles();
                folder.SubFolders.Add(newSubFolder);
            }
        }

        foreach (var subFolder in folder.SubFolders)
        {
            CheckForNewFolders(subFolder);
        }
    }
}