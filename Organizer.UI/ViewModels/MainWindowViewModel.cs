﻿using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.ViewModels.Interfaces;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

namespace Organizer.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private Func<IFriendDetailsViewModel> _friendDetailsViewModelCreator;
        private IMessageService _messageService;
        private Func<IMeetingDetailsViewModel> _meetingDetailsViewModelCreator;
        private IDetailsViewModel _detailsViewModel;
        private IDetailsViewModel _selectedViewModel;

        public MainWindowViewModel(IFriendsListViewModel friendsListViewModel, Func<IFriendDetailsViewModel> friendDetailsViewModelCreator, 
            IEventAggregator eventAggregator, IMessageService msgService, Func<IMeetingDetailsViewModel> meetingDetailsViewModelCreator)
        {
            FriendsListViewModel = friendsListViewModel;
            _eventAggregator = eventAggregator;
            _friendDetailsViewModelCreator = friendDetailsViewModelCreator;
            _messageService = msgService;

            _meetingDetailsViewModelCreator = meetingDetailsViewModelCreator;

            _eventAggregator.GetEvent<ListItemChosenEvent>().Subscribe(OnListItemChosen);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Subscribe(OnDetailDeletedEvent);
            _eventAggregator.GetEvent<TabClosedEvent>().Subscribe(OnTabClosedEvent);

            CreateNewFriendCommand = new DelegateCommand(OnCreateNewFriendCommand);
            CreateNewMeetingCommand = new DelegateCommand(OnCreateNewMeetingCommand);

            DetailViewModels = new ObservableCollection<IDetailsViewModel>();
        }

        public DelegateCommand CreateNewFriendCommand { get; }
        public DelegateCommand CreateNewMeetingCommand { get; }

        public async Task LoadDataAsync()
        {
            await FriendsListViewModel.LoadDataAsync();
        }

        public IFriendsListViewModel FriendsListViewModel { get; }

        public ObservableCollection<IDetailsViewModel> DetailViewModels { get; }

        public IDetailsViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnProperyChanged(nameof(SelectedViewModel));
            }
        }

        public IDetailsViewModel DetailsViewModel
        {
            get { return _detailsViewModel; }
            set
            {
                _detailsViewModel = value;
                OnProperyChanged(nameof(DetailsViewModel));
            }
        }

        private async void OnListItemChosen(ListItemChosenEventArgs eventArgs)
        {
            if (!DetailViewModels.Any(f => f.Id == eventArgs.Id))
            {
                switch (eventArgs.ViewModelName)
                {
                    case nameof(FriendDetailsViewModel):
                        DetailsViewModel = _friendDetailsViewModelCreator();
                        await DetailsViewModel.LoadDetailAsync(eventArgs.Id);
                        DetailViewModels.Add(DetailsViewModel);
                        break;
                    case nameof(MeetingDetailsViewModel):
                        DetailsViewModel = _meetingDetailsViewModelCreator();
                        await DetailsViewModel.LoadDetailAsync(eventArgs.Id);
                        DetailViewModels.Add(DetailsViewModel);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                DetailsViewModel = DetailViewModels.FirstOrDefault(f => f.Id == eventArgs.Id);
            }

            SelectedViewModel = DetailsViewModel;
        }

        private void OnCreateNewFriendCommand()
        {
            OnListItemChosen(new ListItemChosenEventArgs { ViewModelName = nameof(FriendDetailsViewModel) });
        }

        private void OnDetailDeletedEvent(DetailDeletedEventArgs obj)
        {
            DetailsViewModel = null;
        }

        private void OnTabClosedEvent(TabClosedEventArgs args)
        {
            DetailViewModels.Remove(args.DetailViewModel);
        }

        private void OnCreateNewMeetingCommand()
        {
            OnListItemChosen(new ListItemChosenEventArgs { ViewModelName = nameof(MeetingDetailsViewModel) });
        }
    }
}
