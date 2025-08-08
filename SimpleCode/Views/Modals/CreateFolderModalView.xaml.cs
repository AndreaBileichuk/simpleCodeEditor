using System.Windows;
using System.Windows.Input;

namespace SimpleCode.Views.Modals;

public partial class CreateFolderModalView : Window
{
    public CreateFolderModalView()
    {
        InitializeComponent();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
}