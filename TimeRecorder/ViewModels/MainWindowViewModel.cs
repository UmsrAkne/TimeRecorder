using System.Collections.Generic;
using Prism.Mvvm;
using TimeRecorder.Models;

namespace TimeRecorder.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly TimeStampGroup currentGroup;
        private string title = "Prism Application";
        private List<TimeStamp> timeStamps;

        public MainWindowViewModel()
        {
            currentGroup = GetDatabaseContext().GetLatestGroup();
        }

        public string Title { get => title; set => SetProperty(ref title, value); }

        public List<TimeStamp> TimeStamps { get => timeStamps; private set => SetProperty(ref timeStamps, value); }

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