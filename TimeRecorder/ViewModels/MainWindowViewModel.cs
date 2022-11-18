using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using TimeRecorder.Models;

namespace TimeRecorder.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private TimeStampGroup currentGroup;

        private string title = "Prism Application";
        private List<TimeStamp> timeStamps;

        private DelegateCommand<string> addTimeStampCommand;
        private DelegateCommand prevHistoryCommand;
        private DelegateCommand nextHistoryCommand;
        private DelegateCommand latestHistoryCommand;

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

        private void UpdateTimeStamps()
        {
            if (!ShowActiveEventTimeStamp)
            {
                // 大文字小文字関係なく、 activated がコメントに含まれるタイムスタンプをリストから除く
                TimeStamps = GetDatabaseContext().GetTimeStamps(currentGroup)
                    .Where(t => !t.Comment.Contains("activated", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                TimeStamps = GetDatabaseContext().GetTimeStamps(currentGroup);
            }
        }

        private DatabaseContext GetDatabaseContext()
        {
            var context = new DatabaseContext();
            context.Database.EnsureCreated();
            return context;
        }
    }
}