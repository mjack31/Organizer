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
        
        // do konstruktora przekazujemy wszystkie obiekty na których chcemy pracować - IoC
        public FriendDetailsViewModel(IFriendsRepository<Friend> friendDataService, IMessageService msgService, IProgLangLookupItemsDataService progLangDataService,
            IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _friendDataService = friendDataService;
            
            _messageService = msgService;
            _progLangDataService = progLangDataService;

            // inicjalizacja property SaveCommand - konstruktor przyjmuje 2 delegaty/metody. Dobrą praktyką jest nazwyać pierwszą z "on" na początku bo to handler
            // a drugą z CanExecute na końcu ponieważ ona zezwala na odpalenie handlera - wyszaża przycisk
            AddNumberCommand = new DelegateCommand(OnAddNumberCommand);
            DeleteNumberCommand = new DelegateCommand(OnDeleteNumberCommand, OnDeleteNumberCommandCanExecute);

            ProgLangList = new ObservableCollection<ListItem>();
            PhoneNumbers = new ObservableCollection<PhoneNumberWrapper>();
        }

        public DelegateCommand AddNumberCommand { get; }
        public DelegateCommand DeleteNumberCommand { get; }

        public ObservableCollection<ListItem> ProgLangList { get; set; }
        public ObservableCollection<PhoneNumberWrapper> PhoneNumbers { get; set; }

        // propery do którego zbindowany jest widok
        public FriendWrapper Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                // przy każdym setowaniu odpalać ten event aby UI się updateowało
                // tutaj UI musi wiedzieć że wybrano jakiegoś Frienda i że wybrano innego nawet jeżeli tworzony jest nowy ViewModel
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

        // ładowanie pełnych danych wybranego przyjaciela
        public override async Task LoadDetailAsync(int? friendId)
        {
            await LoadFriend(friendId);

            await LoadProgLanguages();

            LoadPhoneNumbers();

            // sprawdzenie czy można zapisać zmiany
            SaveCommand.RaiseCanExecuteChanged();
        }

        protected override void OnDeleteCommand()
        {
            var result = _messageService.ShowOKCancelMsg("Do you want to delete a friend?");
            if (!result)
            {
                return;
            }
            _friendDataService.Delete(Friend.Model);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Publish(new DetailDeletedEventArgs { Id = Friend.Id, ViewModelName = nameof(Friend)});
        }

        protected override async void OnSaveCommand()
        {
            await _friendDataService.SaveAsync();
            _eventAggregator.GetEvent<DetailChangesSavedEvent>().Publish(new DetailChangesSavedEventArgs
            {
                Id = Friend.Id,
                Name = $"{Friend.FirstName} {Friend.LastName}",
                ViewModelName = nameof(Friend)
            });
            // wylączenie przycisku Save po zapisaniu poprzes sprawdzenie zmian w kontekscie
            HasChanges = _friendDataService.HasChanges();
        }

        protected override bool OnSaveCoommandCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges && !PhoneNumbers.Any(f => f.HasErrors);
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
                // normalnie metoda GetFriendAsync nie przyjmuje nullable, ale wystarczy przekazać value friendId i działa
                var friend = await _friendDataService.GetAsync(friendId.Value);
                Friend = new FriendWrapper(friend);
            }
            else
            {
                // tworzenie nowego frienda
                var friend = CreateNewFriend();
                Friend = new FriendWrapper(friend);
            }

            Friend.PropertyChanged += (s, e) =>
            {
                // przy każdej zmianie propery w wrapperze odpalenie eventu
                SaveCommand.RaiseCanExecuteChanged();
                // sprawdzanie kontekstu db czy są zmiany
                HasChanges = _friendDataService.HasChanges();
            };

            // sztuczka aby zaraz po naciśnięciu przycisku utworzenia nowego frienda odświerzył się widok formularza i pojawiły się wskazówki walidacyjne
            if (!friendId.HasValue)
            {
                Friend.LastName = "";
                Friend.FirstName = "";
            }
        }

        private Friend CreateNewFriend()
        {
            // stworzenie nowego pustego frienda i przekazanie do do kontekstu db
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
