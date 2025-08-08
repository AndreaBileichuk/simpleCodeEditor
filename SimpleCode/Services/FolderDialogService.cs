namespace SimpleCode.Services;

using System.Windows.Forms;

public class FolderDialogService
{
    public string OpenFolderDialog()
    {
        var dialog = new FolderBrowserDialog();
        dialog.Description = "Select a project folder";

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            return dialog.SelectedPath;
        }

        return string.Empty;
    }
}