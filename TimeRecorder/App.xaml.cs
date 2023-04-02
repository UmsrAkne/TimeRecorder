using System.IO;
using System.Windows;
using Prism.Ioc;
using TimeRecorder.Models;
using TimeRecorder.ViewModels;
using TimeRecorder.Views;

namespace TimeRecorder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
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
            if (!File.Exists(ApplicationSetting.AppSettingFileName))
            {
                ApplicationSetting.WriteApplicationSetting(new ApplicationSetting());
            }

            var context = DatabaseContext;
            var appSetting = ApplicationSetting.ReadApplicationSetting(ApplicationSetting.AppSettingFileName);

            var timeStamp = new TimeStamp() { Comment = appSetting.RunAppMessage };
            context.AddNewGroup(timeStamp);

            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<EditPage, EditPageViewModel>();
            containerRegistry.RegisterDialog<SettingPage, SettingPageViewModel>();
        }
    }
}