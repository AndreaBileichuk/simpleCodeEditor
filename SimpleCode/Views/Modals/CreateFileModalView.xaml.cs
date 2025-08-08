using System.Windows;
using System.Windows.Input;

namespace SimpleCode.Views.Modals;

public partial class CreateFileModalView : Window
{
    public CreateFileModalView()
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