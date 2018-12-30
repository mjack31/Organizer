using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Events;
using Organizer.UI.Services;
using Organizer.UI.Wrappers;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class FriendDetailsViewModel : BaseViewModel, IFriendDetailsViewModel
    {
        private FriendWrapper _friend;
        private IFriendsRepository _friendDataService;
        private IEventAggregator _eventAggregator;
        private IMessageService _messageService;
        private bool _hasChanges;

        // do konstruktora przekazujemy wszystkie obiekty na których chcemy pracować - IoC
        public FriendDetailsViewModel(IFriendsRepository friendDataService, IEventAggregator eventAggregator, IMessageService msgService)
        {
            _friendDataService = friendDataService;
            _eventAggregator = eventAggregator;
            _messageService = msgService;

            // inicjalizacja property SaveCommand - konstruktor przyjmuje 2 delegaty/metody. Dobrą praktyką jest nazwyać pierwszą z "on" na początku bo to handler
            // a drugą z CanExecute na końcu ponieważ ona zezwala na odpalenie handlera - wyszaża przycisk
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCoommandCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteCommand);
        }

        // command przycisku zapisz zmiany
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                _hasChanges = value;
                // informacja o zmianie tego prop raczej nie potrzebna bo i tak jest odpalany event RaiseCanExecuteChanged
                OnProperyChanged(nameof(HasChanges));
                // event odpalany po to aby zaktualizowac przycisk save
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

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

        // ładowanie pełnych danych wybranego przyjaciela
        public async Task LoadFriendAsync(int? friendId)
        {
            if (friendId.HasValue)
            {
                // normalnie metoda GetFriendAsync nie przyjmuje nullable, ale wystarczy przekazać value friendId i działa
                var friend = await _friendDataService.GetFriendAsync(friendId.Value);
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
                Friend.FirstName = "";
            }

            // sprawdzenie czy można zapisaćzmiany
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void OnDeleteCommand()
        {
            var result = _messageService.ShowOKCancelMsg("Do you want to delete a friend?");
            if (!result)
            {
                return;
            }
            _friendDataService.Delete(Friend.Model);
            _eventAggregator.GetEvent<FriendDeletedEvent>().Publish(Friend.Id);
        }

        private async void OnSaveCommand()
        {
            await _friendDataService.SaveFriendAsync();
            _eventAggregator.GetEvent<FriendChangesSavedEvent>().Publish(new ListItem { Id = Friend.Id, Name = $"{Friend.FirstName} {Friend.LastName}" });
            // wylączenie przycisku Save po zapisaniu poprzes sprawdzenie zmian w kontekscie
            HasChanges = _friendDataService.HasChanges();
        }

        private bool OnSaveCoommandCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private Friend CreateNewFriend()
        {
            // stworzenie nowego pustego frienda i przekazanie do do kontekstu db
            var friend = new Friend();
            return _friendDataService.Add(friend);
        }
    }
}
