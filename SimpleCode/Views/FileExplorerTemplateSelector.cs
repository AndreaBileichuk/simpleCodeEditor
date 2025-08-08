using System.Windows;
using System.Windows.Controls;

namespace SimpleCode.Views
{
    public class FileExplorerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderTemplate { get; set; }
        public DataTemplate FileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ViewModels.FolderViewModel)
                return FolderTemplate;
            if (item is ViewModels.FileItemViewModel)
                return FileTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}