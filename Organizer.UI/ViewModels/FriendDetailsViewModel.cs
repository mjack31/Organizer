using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.Wrappers;
using Prism.Commands;
using Prism.Events;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Organizer.UI.ViewModels.Interfaces;

namespace Organizer.UI.ViewModels
{
    public class FriendDetailsViewModel : DetailViewModelBase, IFriendDetailsViewModel
    {
        private FriendWrapper _friend;
        private IFriendsRepository<Friend> _friendDataService;
        private IMessageService _messageService;
        private IProgLangLookupItemsDataService _progLangDataService;
        private PhoneNumberWrapper _selectedPhoneNumber;
        
        public FriendDetailsViewModel(IFriendsRepository<Friend> friendDataService, IMessageService msgService, IProgLangLookupItemsDataService progLangDataService,
            IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _friendDataService = friendDataService;
            
            _messageService = msgService;
            _progLangDataService = progLangDataService;

            AddNumberCommand = new DelegateCommand(OnAddNumberCommand);
            DeleteNumberCommand = new DelegateCommand(OnDeleteNumberCommand, OnDeleteNumberCommandCanExecute);

            ProgLangList = new ObservableCollection<ListItem>();
            PhoneNumbers = new ObservableCollection<PhoneNumberWrapper>();
        }

        public DelegateCommand AddNumberCommand { get; }
        public DelegateCommand DeleteNumberCommand { get; }

        public ObservableCollection<ListItem> ProgLangList { get; set; }
        public ObservableCollection<PhoneNumberWrapper> PhoneNumbers { get; set; }

        public FriendWrapper Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                OnProperyChanged(nameof(Friend));
            }
        }

        public PhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnProperyChanged(nameof(SelectedPhoneNumber));
                DeleteNumberCommand.RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadDetailAsync(int? friendId)
        {
            await LoadFriend(friendId);

            await LoadProgLanguages();

            LoadPhoneNumbers();

            SaveCommand.RaiseCanExecuteChanged();

            Name = $"{Friend.FirstName} {Friend.LastName}";
        }

        protected override void OnDeleteCommand()
        {
            var result = _messageService.ShowOKCancelMsg("Do you want to delete a friend?");
            if (!result)
            {
                return;
            }
            _friendDataService.Delete(Friend.Model);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Publish(new DetailDeletedEventArgs { Id = Friend.Id, ViewModelName = nameof(FriendDetailsViewModel) });
            OnCloseTabCommand();
        }

        protected override async void OnSaveCommand()
        {
            await _friendDataService.SaveAsync();
            _eventAggregator.GetEvent<DetailChangesSavedEvent>().Publish(new DetailChangesSavedEventArgs
            {
                Id = Friend.Id,
                Name = $"{Friend.FirstName} {Friend.LastName}",
                ViewModelName = GetType().Name
            });

            HasChanges = _friendDataService.HasChanges();
            Name = $"{Friend.FirstName} {Friend.LastName}";
        }

        protected override bool OnSaveCoommandCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges && !PhoneNumbers.Any(f => f.HasErrors);
        }

        protected override void OnCloseTabCommand()
        {
            if (HasChanges)
            {
                var result = _messageService.ShowOKCancelMsg("Do you want to close a friend without save?");
                if (!result)
                {
                    return;
                }
            }
            _eventAggregator.GetEvent<TabClosedEvent>().Publish(new TabClosedEventArgs { DetailViewModel = this });
        }

        private void LoadPhoneNumbers()
        {
            PhoneNumbers.Clear();
            foreach (var number in Friend.PhoneNumbers)
            {
                var wrappedNumber = new PhoneNumberWrapper(number);
                wrappedNumber.PropertyChanged += WrappedNumber_PropertyChanged;
                PhoneNumbers.Add(wrappedNumber);
            }
        }

        private void WrappedNumber_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
            HasChanges = _friendDataService.HasChanges();
        }

        private async Task LoadProgLanguages()
        {
            var languagesList = await _progLangDataService.GetAllProgLangAsync();
            ProgLangList.Clear();
            ProgLangList.Add(new NullListItem { Name = "-" });
            foreach (var lang in languagesList)
            {
                ProgLangList.Add(new ListItem { Id = lang.Id, Name = lang.Name });
            }
        }

        private async Task LoadFriend(int? friendId)
        {
            if (friendId.HasValue)
            {
                var friend = await _friendDataService.GetAsync(friendId.Value);
                Friend = new FriendWrapper(friend);
            }
            else
            {
                var friend = CreateNewFriend();
                Friend = new FriendWrapper(friend);
            }

            Friend.PropertyChanged += (s, e) =>
            {
                SaveCommand.RaiseCanExecuteChanged();
                HasChanges = _friendDataService.HasChanges();
            };

            if (!friendId.HasValue)
            {
                Friend.LastName = "";
                Friend.FirstName = "";
            }

            Id = Friend.Id;
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            return _friendDataService.Add(friend);
        }

        private void OnDeleteNumberCommand()
        {
            var numberToDel = Friend.Model.PhoneNumbers.Where(f => f.Number == SelectedPhoneNumber.Number).FirstOrDefault();
            _friendDataService.RemovePhoneNumber(numberToDel);
            LoadPhoneNumbers();
            HasChanges = _friendDataService.HasChanges();
        }

        private bool OnDeleteNumberCommandCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private void OnAddNumberCommand()
        {
            var newNumber = new PhoneNumber();
            Friend.Model.PhoneNumbers.Add(newNumber);
            LoadPhoneNumbers();
        }
    }
}
