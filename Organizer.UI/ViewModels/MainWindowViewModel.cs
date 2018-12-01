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
    public class MainWindowViewModel : BaseViewModel
    {
        private Friend _selectedFriend;
        private IFriendsDataService _friendsDataService;

        public MainWindowViewModel(IFriendsDataService friendsDataService)
        {
            Friends = new ObservableCollection<Friend>();
            _friendsDataService = friendsDataService;
        }

        public void LoadData()
        {
            var friends = _friendsDataService.GetAll();
            Friends.Clear(); // Dla pewności zawsze czyścić kolekcję
            foreach (Friend friend in friends)
            {
                Friends.Add(friend);
            }
        }

        public ObservableCollection<Friend> Friends { get; set; }

        public Friend SelectedFriend
        {
            get { return _selectedFriend; }
            set {
                _selectedFriend = value;
                // przy każdym setowaniu odpalać ten event aby UI się updateowało
                OnProperyChanged(nameof(SelectedFriend));
            }
        }
    }
}
