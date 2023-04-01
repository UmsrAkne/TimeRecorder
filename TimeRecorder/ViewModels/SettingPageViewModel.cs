using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TimeRecorder.Models;

namespace TimeRecorder.ViewModels;

public class SettingPageViewModel : BindableBase, IDialogAware
{
    public event Action<IDialogResult> RequestClose;

    public string Title => string.Empty;

    public ApplicationSetting ApplicationSetting { get; } = new ApplicationSetting();

    public DelegateCommand CloseCommand => new DelegateCommand(() =>
    {
        RequestClose?.Invoke(new DialogResult());
    });

    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
    }
}