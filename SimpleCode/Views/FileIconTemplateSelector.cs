using System.IO;
using System.Windows;
using System.Windows.Controls;
using SimpleCode.ViewModels;

namespace SimpleCode.Views;

public class FileIconTemplateSelector : DataTemplateSelector
{
    public DataTemplate CppIcon { get; set; }
    public DataTemplate CIcon { get; set; }
    public DataTemplate DefaultIcon { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is string input)
        {
            string extension = input?.ToLower();
            return extension switch
            {
                ".cpp" or ".hpp" or ".h" => CppIcon,
                ".c" => CIcon,
                _ => DefaultIcon
            };
        }

        return DefaultIcon;
    }
}