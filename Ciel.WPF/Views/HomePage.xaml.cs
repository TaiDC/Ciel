using System.Windows.Controls;

using Ciel.WPF.ViewModels;

namespace Ciel.WPF.Views;

public partial class HomePage : Page
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
