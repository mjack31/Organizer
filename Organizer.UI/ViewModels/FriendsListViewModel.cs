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
        private ILookupItemsDataService _friendsDataService;
        private IEventAggregator _eventAggregator;

        // do konstruktora przekazujemy wszystkie obiekty na których chcemy pracować - IoC
        public FriendsListViewModel(ILookupItemsDataService friendsDataService, IEventAggregator eventAggregator)
        {
            // unikać instancjowania obiektów przez new, chyba że sąto listy, słowniki itp
            FriendsList = new ObservableCollection<ListItemViewModel>();
            _friendsDataService = friendsDataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<DetailChangesSavedEvent>().Subscribe(OnFriendChangesSaved);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Subscribe(OnFriendDeletedEvent);
        }

        // Aktywna lista która updateuje UI gdy zmienia się jej zawartość
        // Do tego propery zbindowany jest ListView
        public ObservableCollection<ListItemViewModel> FriendsList { get; }

        // ładowanie listy przyjaciół
        public async Task LoadDataAsync()
        {
            var listItems = await _friendsDataService.GetAllFriendsAsync();
            FriendsList.Clear(); // Dla pewności zawsze czyścić kolekcję
            foreach (var item in listItems)
            {
                var friend = new ListItemViewModel(item.Id, item.Name, _eventAggregator, nameof(FriendDetailsViewModel));
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

        private void OnFriendDeletedEvent(DetailDeletedEventArgs eventArgs)
        {
            var friendToDelete = FriendsList.SingleOrDefault(f => f.Id == eventArgs.Id);
            FriendsList.Remove(friendToDelete);
        }

        private void OnFriendChangesSaved(DetailChangesSavedEventArgs eventArgs)
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
    }
}
