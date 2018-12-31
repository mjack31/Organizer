using Autofac;
using Organizer.DataAccess;
using Organizer.UI.Data;
using Organizer.UI.Services;
using Organizer.UI.ViewModels;
using Prism.Events;
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

            //prism event aggregator
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            //rejestrowanie klas kontenera
            builder.RegisterType<LookupItemsDataService>().As<ILookupItemsDataService>();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<OrganizerDbContext>().AsSelf();
            builder.RegisterType<FriendsRepository>().As<IFriendsRepository>();
            builder.RegisterType<FriendsListViewModel>().As<IFriendsListViewModel>();
            builder.RegisterType<FriendDetailsViewModel>().As<IFriendDetailsViewModel>();
            builder.RegisterType<PopUpMessageService>().As<IMessageService>();
            builder.RegisterType<LookupItemsDataService>().As<IProgLangLookupItemsDataService>();

            return builder.Build();
        }
    }
}
