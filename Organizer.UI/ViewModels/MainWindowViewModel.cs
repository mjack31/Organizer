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
        public MainWindowViewModel(IFriendsListViewModel friendsListViewModel, IFriendDetailsViewModel friendDetailsViewModel)
        {
            FriendsListViewModel = friendsListViewModel;
            FriendDetailsViewModel = friendDetailsViewModel;
        }

        public async Task LoadDataAsync()
        {
            await FriendsListViewModel.LoadDataAsync();
        }

        public IFriendsListViewModel FriendsListViewModel { get; }
        public IFriendDetailsViewModel FriendDetailsViewModel { get; }
    }
}
