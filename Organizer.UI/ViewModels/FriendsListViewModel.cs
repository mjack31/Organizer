using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public class FriendsListViewModel : BaseViewModel, IFriendsListViewModel
    {
        private IListItemsDataService _friendsDataService;
        private IEventAggregator _eventAggregator;

        // do konstruktora przekazujemy wszystkie obiekty na których chcemy pracować - IoC
        public FriendsListViewModel(IListItemsDataService friendsDataService, IEventAggregator eventAggregator)
        {
            // unikać instancjowania obiektów przez new, chyba że sąto listy, słowniki itp
            FriendsList = new ObservableCollection<ListItemViewModel>();
            _friendsDataService = friendsDataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<FriendChangesSavedEvent>().Subscribe(OnFriendChangesSavedAsync);
            _eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnFriendDeletedEvent);
        }

        private void OnFriendDeletedEvent(int id)
        {
            var friendToDelete = FriendsList.SingleOrDefault(f => f.Id == id);
            FriendsList.Remove(friendToDelete);
        }

        private void OnFriendChangesSavedAsync(ListItem friend)
        {
            var friendToChange = FriendsList.SingleOrDefault(f => f.Id == friend.Id);
            if (friendToChange == null)
            {
                FriendsList.Add(new ListItemViewModel(friend.Id, friend.Name, _eventAggregator));
            }
            else
            {
                friendToChange.Name = friend.Name;
            }
        }

        // Aktywna lista która updateuje UI gdy zmienia się jej zawartość
        // Do tego propery zbindowany jest ListView
        public ObservableCollection<ListItemViewModel> FriendsList { get; }

        // ładowanie listy przyjaciół
        public async Task LoadDataAsync()
        {
            var listItems = await _friendsDataService.GetAllAsync();
            FriendsList.Clear(); // Dla pewności zawsze czyścić kolekcję
            foreach (var item in listItems)
            {
                var friend = new ListItemViewModel(item.Id, item.Name, _eventAggregator);
                FriendsList.Add(friend);
            }
        }

        // Po zmianie z ListView na buttony i przeniesieniu publishera eventu kod nie jest już potrzebny
        //// property do którego zbindowanne jest SelectedItem elementu ListView widoku
        //private ListItemViewModel _selectedFriend;
        //public ListItemViewModel SelectedFriend
        //{
        //    get { return _selectedFriend; }
        //    set
        //    {
        //        _selectedFriend = value;

        //        // odpalenie eventu informującego że zaznaczono innego frienda
        //        _eventAggregator.GetEvent<ListItemChosenEvent>().Publish(SelectedFriend.Id);
        //    }
        //}
    }
}
