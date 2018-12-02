using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class FriendsListViewModel : BaseViewModel, IFriendsListViewModel
    {
        private IFriendsDataService _friendsDataService;
        private IEventAggregator _eventAggregator;

        // do konstruktora przekazujemy wszystkie obiekty na których chcemy pracować - IoC
        public FriendsListViewModel(IFriendsDataService friendsDataService, IEventAggregator eventAggregator)
        {
            // unikać instancjowania obiektów przez new, chyba że sąto listy, słowniki itp
            FriendsList = new ObservableCollection<ListItem>();
            _friendsDataService = friendsDataService;
            _eventAggregator = eventAggregator;
        }

        // Aktywna lista która updateuje UI gdy zmienia się jej zawartość
        // Do tego propery zbindowany jest ListView
        public ObservableCollection<ListItem> FriendsList { get; set; }

        // ładowanie listy przyjaciół
        public async Task LoadDataAsync()
        {
            var listItems = await _friendsDataService.GetAllAsync();
            FriendsList.Clear(); // Dla pewności zawsze czyścić kolekcję
            foreach (ListItem item in listItems)
            {
                FriendsList.Add(item);
            }
        }

        // property do którego zbindowanne jest SelectedItem elementu ListView widoku
        private ListItem _selectedFriend;
        public ListItem SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                // przy każdym setowaniu odpalać ten event aby UI się updateowało
                // tutaj chyba nie musi być UI powiadamiane o zmianie
                // OnProperyChanged(nameof(SelectedFriend));

                // odpalenie event z EventAggregatora Prism'a. Klasa ListItemChosenEvent definiuje że payload jest typu int
                _eventAggregator.GetEvent<ListItemChosenEvent>().Publish(SelectedFriend.Id);
            }
        }

    }
}
