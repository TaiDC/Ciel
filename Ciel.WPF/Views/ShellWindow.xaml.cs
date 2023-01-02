using System.Windows.Controls;

using Ciel.WPF.Contracts.Views;
using Ciel.WPF.ViewModels;

using MahApps.Metro.Controls;

namespace Ciel.WPF.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame()
        => shellFrame;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
