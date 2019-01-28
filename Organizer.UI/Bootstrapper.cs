using Autofac;
using Organizer.DataAccess;
using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Data.Interfaces;
using Organizer.UI.Services;
using Organizer.UI.ViewModels;
using Organizer.UI.ViewModels.Interfaces;
using Prism.Events;

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
            builder.RegisterType<FriendsRepository>().As<IFriendsRepository<Friend>>();
            builder.RegisterType<FriendsListViewModel>().As<IFriendsListViewModel>();
            builder.RegisterType<FriendDetailsViewModel>().As<IFriendDetailsViewModel>();
            builder.RegisterType<PopUpMessageService>().As<IMessageService>();
            builder.RegisterType<LookupItemsDataService>().As<IProgLangLookupItemsDataService>();

            builder.RegisterType<MeetingDetailsViewModel>().As<IMeetingDetailsViewModel>();
            builder.RegisterType<MeetingsRepository>().As<IMeetingsRepository<Meeting>>();
            return builder.Build();
        }
    }
}
