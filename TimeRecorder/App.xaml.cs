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
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = new DatabaseContext();
            context.Database.EnsureCreated();

            var group = new TimeStampGroup() { DateTime = DateTime.Now };
            context.Add(group);

            var timeStamp = new TimeStamp() { Comment = "アプリ起動", GroupId = group.Id};
            context.Add(timeStamp);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}