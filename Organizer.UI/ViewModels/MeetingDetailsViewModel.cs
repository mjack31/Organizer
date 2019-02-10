using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Data.Interfaces;
using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.Wrappers;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
                // normalnie metoda GetAsync nie przyjmuje nullable, ale wystarczy przekazać value id i działa
                var meeting = await _meetingsRepo.GetAsync(id.Value);
                Meeting = new MeetingWrapper(meeting);
            }
            else
            {
                // tworzenie nowego spotkania
                var meeting = CreateNewMeeting();
                Meeting = new MeetingWrapper(meeting);
            }

            Meeting.PropertyChanged += (s, e) =>
            {
                // przy każdej zmianie propery w wrapperze odpalenie eventu
                SaveCommand.RaiseCanExecuteChanged();
                // sprawdzanie kontekstu db czy są zmiany
                HasChanges = _meetingsRepo.HasChanges();
            };

            // sztuczka aby zaraz po naciśnięciu przycisku utworzenia nowego meetinga odświerzył się widok formularza i pojawiły się wskazówki walidacyjne
            if (!id.HasValue)
            {
                Meeting.Title = "";
                Meeting.FromDate = DateTime.Today;
                Meeting.ToDate = DateTime.Today;
            }

            InitialiseMeetingFriends();

            // sprawdzenie czy można zapisać zmiany
            SaveCommand.RaiseCanExecuteChanged();
        }

        protected override void OnDeleteCommand()
        {
            var result = _messageService.ShowOKCancelMsg("Do you want to delete a meeting?");
            if (!result)
            {
                return;
            }
            _meetingsRepo.Delete(Meeting.Model);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Publish(new DetailDeletedEventArgs { Id = Meeting.Id, ViewModelName = nameof(Meeting) });
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
            // wylączenie przycisku Save po zapisaniu poprzes sprawdzenie zmian w kontekscie
            HasChanges = _meetingsRepo.HasChanges();
        }

        protected override bool OnSaveCoommandCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
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
            // stworzenie nowego pustego spotkania i przekazanie do do kontekstu db
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
