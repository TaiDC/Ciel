﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ciel.WPF.ViewModels;

public partial class ShellDialogViewModel : ObservableObject
{
    public Action<bool?> SetResult { get; set; }

    public ShellDialogViewModel()
    {
    }

    [RelayCommand]
    private void OnClose()
    {
        bool result = true;
        SetResult(result);
    }
}
