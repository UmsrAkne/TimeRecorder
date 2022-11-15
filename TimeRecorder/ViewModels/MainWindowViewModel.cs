using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using TimeRecorder.Models;

namespace TimeRecorder.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly TimeStampGroup currentGroup;
        private string title = "Prism Application";
        private List<TimeStamp> timeStamps;

        private DelegateCommand<string> addTimeStampCommand;

        private bool showActiveEventTimeStamp = true;

        public MainWindowViewModel()
        {
            currentGroup = GetDatabaseContext().GetLatestGroup();
            UpdateTimeStamps();
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public List<TimeStamp> TimeStamps { get => timeStamps; private set => SetProperty(ref timeStamps, value); }

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
                var timeStamp = new TimeStamp()
                {
                    Comment = comment,
                    GroupId = currentGroup.Id,
                };

                GetDatabaseContext().Add(timeStamp);
                UpdateTimeStamps();
                Title = timeStamp.DateTime.ToString("MM/dd hh:mm:ss");
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