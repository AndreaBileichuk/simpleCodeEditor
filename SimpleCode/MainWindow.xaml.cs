using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleCode.ViewModels;

namespace SimpleCode;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void MainWindow_OnActivated(object? sender, EventArgs e)
    {
        MainViewModel mainViewModel = DataContext as MainViewModel;

        if (mainViewModel.CurrentViewModel is WorkSpaceViewModel workSpaceViewModel)
        {
            workSpaceViewModel.RefreshCurrentActiveFileContentAfterActivatingWindow();
        }
    }

    private void MainWindow_OnDeactivated(object? sender, EventArgs e)
    {
        MainViewModel mainViewModel = DataContext as MainViewModel;

        if (mainViewModel.CurrentViewModel is WorkSpaceViewModel workSpaceViewModel)
        {
            workSpaceViewModel.CodeWorkspaceViewModel.ActiveFile?.FileModel.SaveChanges();
        }
    }
}