using System.IO;
using System.Windows;

namespace SimpleCode.Models;

public class FileModel
{
    public string Name { get; set; }
    public string FullPath { get; set; }
    public string Content { get; set; }
    public bool IsModified { get; set; }

    public FileModel(string fullPath)
    {
        Name = Path.GetFileName(fullPath);
        FullPath = fullPath;
    }

    public string LoadFileContent()
    {
        IsModified = false;
        if (!string.IsNullOrWhiteSpace(Content))
        {
            return Content;
        }

        try
        {
            StreamReader sr = new StreamReader(FullPath);
            string? line = sr.ReadLine();

            while (line != null)
            {
                //write the line to console window
                Content += (line + "\n");
                //Read the next line
                line = sr.ReadLine();
            }

            sr.Close();
            return Content;
        }
        catch (Exception e)
        {
            MessageBox.Show("Something went wrong\n" + e.Message);
            return "";
        }
    }

    public string LoadFileContentAfterActivation()
    {
        Content = "";

        StreamReader sr = new StreamReader(FullPath);
        string line = sr.ReadLine();

        while (line != null)
        {
            //write the line to console window
            Content += (line + "\n");
            //Read the next line
            line = sr.ReadLine();
        }

        sr.Close();
        return Content;
    }

    public void SaveChanges()
    {
        if (string.IsNullOrEmpty(FullPath))
        {
            throw new InvalidOperationException("File path is not set.");
        }

        try
        {
            using (StreamWriter outputFile = new StreamWriter(FullPath, false))
            {
                outputFile.Write(Content);
            }

            IsModified = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving file: {ex.Message}");
        }
    }

    public void RenameFile(string newName)
    {
        string directory = Path.GetDirectoryName(FullPath);
        string newFullPath = Path.Combine(directory, newName);

        if (File.Exists(FullPath))
        {
            File.Move(FullPath, newFullPath);
        }

        Name = newName;
        FullPath = newFullPath;
    }
}