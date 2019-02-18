using Organizer.Models;
using Organizer.UI.Data.Interfaces;
using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.Wrappers;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class MeetingDetailsViewModel : DetailViewModelBase, IMeetingDetailsViewModel
    {
        private IMessageService _messageService;
        private IMeetingsRepository<Meeting> _meetingsRepo;
        private MeetingWrapper _meeting;
        private Friend _selectedAvaibleFriend;
        private Friend _selectedFriendInMeeting;

        public MeetingDetailsViewModel(IEventAggregator eventAggregator, IMessageService msgService,
            IMeetingsRepository<Meeting> meetingsRepo) : base(eventAggregator)
        {
            _messageService = msgService;
            _meetingsRepo = meetingsRepo;

            FriendsInMeeting = new ObservableCollection<Friend>();
            AvaibleFriends = new ObservableCollection<Friend>();

            AddFriendToMeetingCommand = new DelegateCommand(OnAddFriendToMeetingCmd, OnAddFriendToMeetingCmdCanExecute);
            RemoveFriendFromMeetingCommand = new DelegateCommand(OnRemoveFriendFromMeetingCommand, RemoveFriendFromMeetingCommandCanExecute);
        }

        public DelegateCommand AddFriendToMeetingCommand { get; }
        public DelegateCommand RemoveFriendFromMeetingCommand { get; }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            set
            {
                _meeting = value;
                OnProperyChanged(nameof(Meeting));
            }
        }

        public ObservableCollection<Friend> FriendsInMeeting { get; set; }
        public ObservableCollection<Friend> AvaibleFriends { get; set; }

        public Friend SelectedFriendInMeeting
        {
            get { return _selectedFriendInMeeting; }
            set
            {
                _selectedFriendInMeeting = value;
                RemoveFriendFromMeetingCommand.RaiseCanExecuteChanged();
            }
        }

        public Friend SelectedAvaibleFriend
        {
            get { return  _selectedAvaibleFriend; }
            set
            {
                _selectedAvaibleFriend = value;
                AddFriendToMeetingCommand.RaiseCanExecuteChanged();
            }
        }

        public async override Task LoadDetailAsync(int? id)
        {
            if (id.HasValue)
            {
                var meeting = await _meetingsRepo.GetAsync(id.Value);
                Meeting = new MeetingWrapper(meeting);
            }
            else
            {
                var meeting = CreateNewMeeting();
                Meeting = new MeetingWrapper(meeting);
            }

            Meeting.PropertyChanged += (s, e) =>
            {
                SaveCommand.RaiseCanExecuteChanged();
                HasChanges = _meetingsRepo.HasChanges();
            };

            if (!id.HasValue)
            {
                Meeting.Title = "";
                Meeting.FromDate = DateTime.Today;
                Meeting.ToDate = DateTime.Today;
            }

            InitialiseMeetingFriends();

            SaveCommand.RaiseCanExecuteChanged();

            Name = Meeting.Title;
            Id = Meeting.Id;
        }

        protected override void OnDeleteCommand()
        {
            var result = _messageService.ShowOKCancelMsg("Do you want to delete a meeting?");
            if (!result)
            {
                return;
            }
            _meetingsRepo.Delete(Meeting.Model);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Publish(new DetailDeletedEventArgs { Id = Meeting.Id, ViewModelName = nameof(MeetingDetailsViewModel) });
            OnCloseTabCommand();
        }

        protected async override void OnSaveCommand()
        {
            await _meetingsRepo.SaveAsync();
            _eventAggregator.GetEvent<DetailChangesSavedEvent>().Publish(new DetailChangesSavedEventArgs
            {
                Id = Meeting.Id,
                Name = Meeting.Title,
                ViewModelName = GetType().Name
            });
            HasChanges = _meetingsRepo.HasChanges();
            Name = Meeting.Title;
        }

        protected override bool OnSaveCoommandCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override void OnCloseTabCommand()
        {
            if (HasChanges)
            {
                var result = _messageService.ShowOKCancelMsg("Do you want to close a meeting without save?");
                if (!result)
                {
                    return;
                }
            }
            _eventAggregator.GetEvent<TabClosedEvent>().Publish(new TabClosedEventArgs { DetailViewModel = this });
        }

        private async void InitialiseMeetingFriends()
        {
            FriendsInMeeting.Clear();
            AvaibleFriends.Clear();
            foreach (var friend in Meeting.Model.Friends)
            {
                FriendsInMeeting.Add(friend);
            }

            var allFriends = await _meetingsRepo.GetAllAddedFriends();

            foreach (var friend in allFriends)
            {
                if(!FriendsInMeeting.Any(f => f.Id == friend.Id))
                {
                    AvaibleFriends.Add(friend);
                }
            }
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting();
            return _meetingsRepo.Add(meeting);
        }

        private bool RemoveFriendFromMeetingCommandCanExecute()
        {
            return SelectedFriendInMeeting != null;
        }

        private void OnRemoveFriendFromMeetingCommand()
        {
            Meeting.Model.Friends.Remove(SelectedFriendInMeeting);
            AvaibleFriends.Add(SelectedFriendInMeeting);
            FriendsInMeeting.Remove(SelectedFriendInMeeting);
            HasChanges = _meetingsRepo.HasChanges();
        }

        private void OnAddFriendToMeetingCmd()
        {
            Meeting.Model.Friends.Add(SelectedAvaibleFriend);
            FriendsInMeeting.Add(SelectedAvaibleFriend);
            AvaibleFriends.Remove(SelectedAvaibleFriend);
            HasChanges = _meetingsRepo.HasChanges();
        }

        private bool OnAddFriendToMeetingCmdCanExecute()
        {
            return SelectedAvaibleFriend != null;
        }
    }
}
