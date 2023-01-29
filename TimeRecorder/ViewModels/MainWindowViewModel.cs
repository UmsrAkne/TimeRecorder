using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using TimeRecorder.Models;

namespace TimeRecorder.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private TimeStampGroup currentGroup;

        private string title = "Time Recorder";
        private List<TimeStamp> timeStamps;
        private bool reversOrder;

        private DelegateCommand reversOrderCommand;
        private DelegateCommand<string> addTimeStampCommand;
        private DelegateCommand prevHistoryCommand;
        private DelegateCommand nextHistoryCommand;
        private DelegateCommand latestHistoryCommand;
        private DelegateCommand<IEnumerable> copyTimeStampsCommand;

        private bool showActiveEventTimeStamp = true;

        public MainWindowViewModel()
        {
            currentGroup = GetDatabaseContext().GetLatestGroup();
            LatestGroup = GetDatabaseContext().GetLatestGroup();
            UpdateTimeStamps();
        }

        public string Title { get => title; private set => SetProperty(ref title, value); }

        public List<TimeStamp> TimeStamps { get => timeStamps; private set => SetProperty(ref timeStamps, value); }

        public TimeStampGroup LatestGroup { get; private set; }

        public bool ShowActiveEventTimeStamp
        {
            get => showActiveEventTimeStamp;
            set
            {
                SetProperty(ref showActiveEventTimeStamp, value);
                UpdateTimeStamps();
            }
        }

        public DelegateCommand<string> AddTimeStampCommand =>
            addTimeStampCommand ??= new DelegateCommand<string>(comment =>
            {
                var context = GetDatabaseContext();
                var timeStamp = new TimeStamp()
                {
                    Comment = comment,
                    GroupId = LatestGroup.Id,
                };

                context.Add(timeStamp);
                UpdateTimeStamps();
                Title = timeStamp.DateTime.ToString("MM/dd hh:mm:ss");
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