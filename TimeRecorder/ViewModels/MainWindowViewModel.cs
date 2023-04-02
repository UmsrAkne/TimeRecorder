using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TimeRecorder.Models;
using TimeRecorder.Views;

namespace TimeRecorder.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private readonly IDialogService dialogService;

        private TimeStampGroup currentGroup;

        private string title = "Time Recorder";
        private List<TimeStamp> timeStamps;
        private bool reversOrder;
        private ApplicationSetting appSettings;

        private DelegateCommand reversOrderCommand;
        private DelegateCommand<object> addTimeStampCommand;
        private DelegateCommand addCommentTimeStampCommand;
        private DelegateCommand prevHistoryCommand;
        private DelegateCommand nextHistoryCommand;
        private DelegateCommand latestHistoryCommand;
        private DelegateCommand<IEnumerable> copyTimeStampsCommand;

        private bool showActiveEventTimeStamp;
        private string inputText;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            appSettings = ApplicationSetting.ReadApplicationSetting(ApplicationSetting.AppSettingFileName);
            showActiveEventTimeStamp = appSettings.VisibleActivatedLog;
            currentGroup = GetDatabaseContext().GetLatestGroup();
            LatestGroup = GetDatabaseContext().GetLatestGroup();
            UpdateTimeStamps();
        }

        public string Title { get => title; private set => SetProperty(ref title, value); }

        public List<TimeStamp> TimeStamps { get => timeStamps; private set => SetProperty(ref timeStamps, value); }

        public TimeStampGroup LatestGroup { get; private set; }

        public string InputText { get => inputText; set => SetProperty(ref inputText, value); }

        public bool ShowActiveEventTimeStamp
        {
            get => showActiveEventTimeStamp;
            set
            {
                appSettings.VisibleActivatedLog = value;
                ApplicationSetting.WriteApplicationSetting(appSettings);
                SetProperty(ref showActiveEventTimeStamp, value);
                UpdateTimeStamps();
            }
        }

        public DelegateCommand<object> AddTimeStampCommand =>
            addTimeStampCommand ??= new DelegateCommand<object>(commentType =>
            {
                var c = (CommentType)commentType;

                switch (c)
                {
                    case CommentType.RunApp:
                        AddTimeStamp(appSettings.RunAppMessage);
                        break;
                    case CommentType.Activated:
                        AddTimeStamp(appSettings.ActivatedMessage);
                        break;
                    case CommentType.Deactivated:
                        AddTimeStamp(appSettings.DeactivatedMessage);
                        break;
                    case CommentType.CloseApp:
                        AddTimeStamp(appSettings.CloseAppMessage);
                        break;
                    case CommentType.Default:
                        AddTimeStamp(appSettings.DefaultAutoComment);
                        break;
                }
            });

        public DelegateCommand AddCommentTimeStampCommand =>
            addCommentTimeStampCommand ??= new DelegateCommand(() =>
            {
                var comment = InputText;
                if (string.IsNullOrWhiteSpace(comment))
                {
                    comment = appSettings.DefaultComment;
                }

                AddTimeStamp(comment);
                InputText = string.Empty;
            });

        public DelegateCommand PrevHistoryCommand =>
            prevHistoryCommand ??= new DelegateCommand(() =>
            {
                var groups = GetDatabaseContext().GetGroups();
                var current = groups.FirstOrDefault(g => g.Id == currentGroup.Id);

                if (current == null)
                {
                    return;
                }

                var currentIndex = groups.IndexOf(current);
                currentGroup = groups.ElementAtOrDefault(currentIndex - 1) ?? currentGroup;
                UpdateTimeStamps();
            });

        public DelegateCommand NextHistoryCommand =>
            nextHistoryCommand ??= new DelegateCommand(() =>
            {
                var groups = GetDatabaseContext().GetGroups();
                var current = groups.FirstOrDefault(g => g.Id == currentGroup.Id);

                if (current == null)
                {
                    return;
                }

                var currentIndex = groups.IndexOf(current);
                currentGroup = groups.ElementAtOrDefault(currentIndex + 1) ?? currentGroup;
                UpdateTimeStamps();
            });

        public DelegateCommand LatestHistoryCommand =>
            latestHistoryCommand ??= new DelegateCommand(() =>
            {
                currentGroup = LatestGroup;
                UpdateTimeStamps();
            });

        public DelegateCommand<IEnumerable> CopyTimeStampsCommand =>
            copyTimeStampsCommand ??= new DelegateCommand<IEnumerable>(selectedItemCollection =>
            {
                var writer = new TimeStampWriter();
                var tss = selectedItemCollection.Cast<TimeStamp>().ToList();
                Clipboard.SetDataObject(writer.GetTimeStampString(tss));
            });

        public DelegateCommand ReversOrderCommand =>
            reversOrderCommand ??= new DelegateCommand(() =>
            {
                reversOrder = !reversOrder;
                UpdateTimeStamps();
            });

        public DelegateCommand<TimeStamp> ShowEditPageCommand => new DelegateCommand<TimeStamp>((ts) =>
        {
            // ReSharper disable once UnusedParameter.Local
            var param = new DialogParameters { { nameof(TimeStamp), ts } };
            dialogService.ShowDialog(nameof(EditPage), param, result =>
            {
                if (result.Result == ButtonResult.Yes)
                {
                    GetDatabaseContext().Add(result.Parameters.GetValue<TimeStamp>(nameof(TimeStamp)));
                    UpdateTimeStamps();
                }
            });
        });

        public DelegateCommand ShowSettingPageCommand => new DelegateCommand(() =>
        {
            dialogService.ShowDialog(nameof(SettingPage), new DialogParameters(), _ =>
            {
                appSettings = ApplicationSetting.ReadApplicationSetting(ApplicationSetting.AppSettingFileName);
            });
        });

        private void AddTimeStamp(string comment)
        {
            var context = GetDatabaseContext();
            var timeStamp = new TimeStamp()
            {
                Comment = comment,
                GroupId = LatestGroup.Id,
            };

            context.Add(timeStamp);
            UpdateTimeStamps();
            Title = timeStamp.DateTime.ToString("MM/dd HH:mm:ss");
        }

        private void UpdateTimeStamps()
        {
            List<TimeStamp> timeStampList;
            if (!ShowActiveEventTimeStamp)
            {
                // 大文字小文字関係なく、 activated がコメントに含まれるタイムスタンプをリストから除く
                timeStampList = GetDatabaseContext().GetTimeStamps(currentGroup)
                    .Where(t => !t.Comment.Contains("activated", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                timeStampList = GetDatabaseContext().GetTimeStamps(currentGroup);
            }

            // 直前の要素からの経過時間を入力する。
            // [0] に関しては直前の要素は無いため、[1] 始まりで処理する。
            for (var i = 1; i < timeStampList.Count; i++)
            {
                var current = timeStampList[i];
                var beforeTs = timeStampList[i - 1];

                current.ElapsedTime = current.DateTime - beforeTs.DateTime;
                current.ElapsedTime = TimeSpan.FromSeconds(Math.Floor(current.ElapsedTime.TotalSeconds));
            }

            if (reversOrder)
            {
                timeStampList.Reverse();
            }

            TimeStamps = timeStampList;
        }

        private DatabaseContext GetDatabaseContext()
        {
            var context = new DatabaseContext();
            context.Database.EnsureCreated();
            return context;
        }
    }
}