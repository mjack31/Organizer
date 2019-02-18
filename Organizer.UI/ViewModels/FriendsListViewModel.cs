using Organizer.UI.Data;
using Organizer.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class FriendsListViewModel : ViewModelBase, IFriendsListViewModel
    {
        private ILookupItemsDataService _friendsDataService;
        private ILookupItemsDataService _meetingsDataService;
        private IEventAggregator _eventAggregator;

        public FriendsListViewModel(ILookupItemsDataService friendsDataService, IEventAggregator eventAggregator, ILookupItemsDataService meetingsDataService)
        {
            FriendsList = new ObservableCollection<ListItemViewModel>();
            MeetingsList = new ObservableCollection<ListItemViewModel>();

            _friendsDataService = friendsDataService;
            _meetingsDataService = meetingsDataService;

            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<DetailChangesSavedEvent>().Subscribe(OnDetailChangesSaved);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Subscribe(OnDetailDeletedEvent);
        }

        public ObservableCollection<ListItemViewModel> FriendsList { get; }
        public ObservableCollection<ListItemViewModel> MeetingsList { get; }

        public async Task LoadDataAsync()
        {
            await LoadFriends();
            await LoadMeetings();
        }

        private async Task LoadMeetings()
        {
            var listItems = await _meetingsDataService.GetAllMeetingsAsync();
            MeetingsList.Clear();
            foreach (var item in listItems)
            {
                var meeting = new ListItemViewModel(item.Id, item.Name, _eventAggregator, nameof(MeetingDetailsViewModel));
                MeetingsList.Add(meeting);
            }
        }

        private async Task LoadFriends()
        {
            var listItems = await _friendsDataService.GetAllFriendsAsync();
            FriendsList.Clear();
            foreach (var item in listItems)
            {
                var friend = new ListItemViewModel(item.Id, item.Name, _eventAggregator, nameof(FriendDetailsViewModel));
                FriendsList.Add(friend);
            }
        }

        private void OnDetailChangesSaved(DetailChangesSavedEventArgs eventArgs)
        {
            switch (eventArgs.ViewModelName)
            {
                case nameof(FriendDetailsViewModel):
                    OnFriendSave(eventArgs);
                    break;
                case nameof(MeetingDetailsViewModel):
                    OnMeetingSave(eventArgs);
                    break;
                default:
                    throw new Exception("Wrong view model");
            }
        }

        private void OnMeetingSave(DetailChangesSavedEventArgs eventArgs)
        {
            var meetingToChange = MeetingsList.SingleOrDefault(f => f.Id == eventArgs.Id);
            if (meetingToChange == null)
            {
                MeetingsList.Add(new ListItemViewModel(eventArgs.Id, eventArgs.Name, _eventAggregator, nameof(MeetingDetailsViewModel)));
            }
            else
            {
                meetingToChange.Name = eventArgs.Name;
            }
        }

        private void OnFriendSave(DetailChangesSavedEventArgs eventArgs)
        {
            var friendToChange = FriendsList.SingleOrDefault(f => f.Id == eventArgs.Id);
            if (friendToChange == null)
            {
                FriendsList.Add(new ListItemViewModel(eventArgs.Id, eventArgs.Name, _eventAggregator, nameof(FriendDetailsViewModel)));
            }
            else
            {
                friendToChange.Name = eventArgs.Name;
            }
        }

        private void OnDetailDeletedEvent(DetailDeletedEventArgs eventArgs)
        {
            switch (eventArgs.ViewModelName)
            {
                case nameof(FriendDetailsViewModel):
                    var friendToDelete = FriendsList.SingleOrDefault(f => f.Id == eventArgs.Id);
                    FriendsList.Remove(friendToDelete);
                    break;
                case nameof(MeetingDetailsViewModel):
                    var meetingToDelete = MeetingsList.SingleOrDefault(f => f.Id == eventArgs.Id);
                    MeetingsList.Remove(meetingToDelete);
                    break;
                default:
                    throw new Exception("Wrong view model");
            }
        }
    }
}
