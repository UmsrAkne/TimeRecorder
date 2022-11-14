using System.Collections.Generic;
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

        public MainWindowViewModel()
        {
            currentGroup = GetDatabaseContext().GetLatestGroup();
            UpdateTimeStamps();
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public List<TimeStamp> TimeStamps { get => timeStamps; private set => SetProperty(ref timeStamps, value); }

        public DelegateCommand<string> AddTimeStampCommand =>
            addTimeStampCommand ??= new DelegateCommand<string>(comment =>
            {
                GetDatabaseContext().Add(new TimeStamp()
                {
                    Comment = comment,
                    GroupId = currentGroup.Id,
                });

                UpdateTimeStamps();
            });

        private void UpdateTimeStamps()
        {
            TimeStamps = GetDatabaseContext().GetTimeStamps(currentGroup);
        }

        private DatabaseContext GetDatabaseContext()
        {
            var context = new DatabaseContext();
            context.Database.EnsureCreated();
            return context;
        }
    }
}