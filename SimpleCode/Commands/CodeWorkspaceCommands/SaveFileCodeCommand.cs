using System.IO;
using System.Windows.Forms;
using SimpleCode.Models;
using SimpleCode.ViewModels;

namespace SimpleCode.Commands;

public class SaveFileCodeCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        var fileViewModel = parameter as FileItemViewModel;
        fileViewModel?.FileModel.SaveChanges();
    }
}