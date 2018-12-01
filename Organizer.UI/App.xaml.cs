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

            // zwrócenie instancji klasy MainWindow (code behind) i pokazanie okna
            //////////////While it is possible to resolve components right from the root container, doing this through your
            //////////////application in some cases may result in a memory leak.It is recommended you always resolve components 
            //////////////from a lifetime scope where possible to make sure service instances are properly disposed and garbage 
            //////////////collected. You can read more about this in the section on controlling scope and lifetime.
            using (var scope = container.BeginLifetimeScope())
            {
                var mainWindow = scope.Resolve<MainWindow>();
                mainWindow.Show();
            }
        }
    }
}
