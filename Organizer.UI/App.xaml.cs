using Autofac;
using Organizer.UI.Data;
using Organizer.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Organizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // stworzenie bootstrapppera Autoafaca
            var bootsrapper = new Bootstrapper();

            // stworzenie kontenera
            var container = bootsrapper.Container();

            // odpalenie głównego okna
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
