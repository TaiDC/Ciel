using Ciel.WPF.Contracts.Views;
using Ciel.WPF.ViewModels;

using MahApps.Metro.Controls;

namespace Ciel.WPF.Views;

public partial class LogInWindow : MetroWindow, ILogInWindow
{
    public LogInWindow(LogInViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
