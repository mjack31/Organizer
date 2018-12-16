using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Events;
using Organizer.UI.Wrappers;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class FriendDetailsViewModel : BaseViewModel, IFriendDetailsViewModel
    {
        private FriendWrapper _friend;
        private IFriendDetailsDataService _friendDataService;
        private IEventAggregator _eventAggregator;

        // do konstruktora przekazujemy wszystkie obiekty na których chcemy pracować - IoC
        public FriendDetailsViewModel(IFriendDetailsDataService friendDataService, IEventAggregator eventAggregator)
        {
            _friendDataService = friendDataService;
            _eventAggregator = eventAggregator;

            // dodanie subskrybenta (handlera) do eventu EventAggregatora Prism'a. Metoda Subscribe przyjmuje Action<T>
            // ale można zamiast delegaty dodawać normalnie metody - ułatwienie
            _eventAggregator.GetEvent<ListItemChosenEvent>().Subscribe(_onListItemChosen);

            // inicjalizacja property SaveCommand - konstruktor przyjmuje 2 delegaty/metody. Dobrą praktyką jest nazwyać pierwszą z "on" na początku bo to handler
            // a drugą z CanExecute na końcu ponieważ ona zezwala na odpalenie handlera - wyszaża przycisk
            SaveCommand = new DelegateCommand(OnSaveCommand, OnSaveCoommandCanExecute);
        }


        private async void OnSaveCommand()
        {
            await _friendDataService.SaveFriendAsync(Friend.Model);
            _eventAggregator.GetEvent<FriendChangesSavedEvent>().Publish(new ListItem { Id = Friend.Id, Name = $"{Friend.FirstName} {Friend.LastName}" });
        }

        private bool OnSaveCoommandCanExecute()
        {
            // TODO - Dodać warunek
            return Friend != null && !Friend.HasErrors;
        }

        // event handler wybrania danego Friendsa
        private async void _onListItemChosen(int id)
        {
            await LoadFriendAsync(id);
        }

        // ładowanie pełnych danych wybranego przyjaciela
        public async Task LoadFriendAsync(int friendId)
        {
            var friend = await _friendDataService.GetFriendAsync(friendId);
            Friend = new FriendWrapper(friend);

            Friend.PropertyChanged += (s, e) =>
            {
                SaveCommand.RaiseCanExecuteChanged();
            };
        }

        // propery do którego zbindowany jest widok
        public FriendWrapper Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                // przy każdym setowaniu odpalać ten event aby UI się updateowało
                // tutaj UI musi wiedzieć że wybrano jakiegoś Frienda i że wybrano innego
                OnProperyChanged(nameof(Friend));
            }
        }

        // command przycisku zapisz zmiany
        public DelegateCommand SaveCommand { get; private set; }
    }
}
