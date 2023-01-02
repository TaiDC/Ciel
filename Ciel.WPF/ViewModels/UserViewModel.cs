using System.Windows.Media.Imaging;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Ciel.WPF.ViewModels;

public partial class UserViewModel : ObservableObject
{
    [ObservableProperty] private string _userName;

    [ObservableProperty] private string _name;

    [ObservableProperty] private string _mail;

    [ObservableProperty] private BitmapImage _photo;

    public UserViewModel()
    {
    }
}
