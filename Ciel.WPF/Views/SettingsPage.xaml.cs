using System.Windows.Controls;

using Ciel.WPF.ViewModels;

namespace Ciel.WPF.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
