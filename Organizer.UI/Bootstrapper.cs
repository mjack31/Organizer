using Autofac;
using Organizer.DataAccess;
using Organizer.UI.Data;
using Organizer.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI
{
    public class Bootstrapper
    {
        public IContainer Container()
        {
            var builder = new ContainerBuilder();
            //rejestrowanie klas kontenera
            builder.RegisterType<FriendsDbDataService>().As<IFriendsDataService>();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<OrganizerDbContext>().AsSelf();

            return builder.Build();
        }
    }
}
