using System;
using System.Windows;
using Prism.Ioc;
using TimeRecorder.Models;
using TimeRecorder.Views;

namespace TimeRecorder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private int currentGroupId;

        private DatabaseContext DatabaseContext
        {
            get
            {
                var context = new DatabaseContext();
                context.Database.EnsureCreated();
                return context;
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = DatabaseContext;

            var group = new TimeStampGroup() { DateTime = DateTime.Now };
            context.Add(group);
            currentGroupId = group.Id;

            var timeStamp = new TimeStamp() { Comment = "アプリ起動", GroupId = currentGroupId };
            context.Add(timeStamp);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var timeStamp = new TimeStamp() { Comment = "アプリ終了", GroupId = currentGroupId };
            DatabaseContext.Add(timeStamp);

            base.OnExit(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}