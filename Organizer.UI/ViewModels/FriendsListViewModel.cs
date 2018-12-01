using Organizer.Models;
using Organizer.UI.Data;
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

        public FriendsListViewModel(IFriendsDataService friendsDataService)
        {
            FriendsList = new ObservableCollection<ListItem>();
            _friendsDataService = friendsDataService;
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
    }
}
