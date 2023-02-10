using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TimeRecorder.Models;

namespace TimeRecorder.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EditPageViewModel : BindableBase, IDialogAware
    {
        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public TimeStamp CurrentTimeStamp { get; set; }

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
            CurrentTimeStamp = parameters.GetValue<TimeStamp>(nameof(TimeStamp));
        }
    }
}