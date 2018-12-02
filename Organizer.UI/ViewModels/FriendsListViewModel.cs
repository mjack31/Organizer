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

        public FriendsListViewModel(IFriendsDataService friendsDataService, IEventAggregator eventAggregator)
        {
            FriendsList = new ObservableCollection<ListItem>();
            _friendsDataService = friendsDataService;
            _eventAggregator = eventAggregator;
        }

        public ObservableCollection<ListItem> FriendsList { get; set; }

        public async Task LoadDataAsync()
        {
            var listItems = await _friendsDataService.GetAllAsync();
            FriendsList.Clear(); // Dla pewności zawsze czyścić kolekcję
            foreach (ListItem item in listItems)
            {
                FriendsList.Add(item);
            }
        }

        private ListItem _selectedFriend;

        public ListItem SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                // przy każdym setowaniu odpalać ten event aby UI się updateowało
                OnProperyChanged(nameof(SelectedFriend));
                _eventAggregator.GetEvent<ListItemChosenEvent>().Publish(SelectedFriend.Id);
            }
        }

    }
}
