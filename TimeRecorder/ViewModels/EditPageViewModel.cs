using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TimeRecorder.Models;

namespace TimeRecorder.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EditPageViewModel : BindableBase, IDialogAware
    {
        private string dateTimeText;

        public event Action<IDialogResult> RequestClose;

        public string Title => string.Empty;

        public TimeStamp OldTimeStamp { get; set; }

        public TimeStamp CurrentTimeStamp { get; set; }

        public string DateTimeText
        {
            get => dateTimeText;
            set
            {
                var str = Regex.Replace(value, "[ /:_-]", string.Empty);
                var culture = CultureInfo.CurrentCulture;
                const DateTimeStyles styles = DateTimeStyles.None;

                if (DateTime.TryParseExact(str, "yyMMddHHmmss", culture, styles, out var result))
                {
                    CurrentTimeStamp.DateTime = result;
                    SetProperty(ref dateTimeText, value);
                }
                else
                {
                    SetProperty(ref dateTimeText, dateTimeText);
                }
            }
        }

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            var result = new DialogResult(ButtonResult.Cancel);
            RequestClose?.Invoke(result);
        });

        public DelegateCommand ConfirmCommand => new DelegateCommand(() =>
        {
            var result = new DialogResult(ButtonResult.Yes);
            result.Parameters.Add(nameof(TimeStamp), CurrentTimeStamp);
            RequestClose?.Invoke(result);
        });

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            OldTimeStamp = parameters.GetValue<TimeStamp>(nameof(TimeStamp));

            CurrentTimeStamp = new TimeStamp()
            {
                BaseId = OldTimeStamp.Id,
                Comment = OldTimeStamp.Comment,
                GroupId = OldTimeStamp.GroupId,
                DateTime = OldTimeStamp.DateTime,
            };

            DateTimeText = CurrentTimeStamp.DateTime.ToString("yy/MM/dd HH:mm:ss");
        }
    }
}