using Organizer.Models;
using Organizer.UI.Data;
using Organizer.UI.Events;
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
        private Friend _friend;
        private IFriendDetailsDataService _friendDataService;
        private IEventAggregator _eventAggregator;

        public FriendDetailsViewModel(IFriendDetailsDataService friendDataService, IEventAggregator eventAggregator)
        {
            _friendDataService = friendDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ListItemChosenEvent>().Subscribe(onListItemChosen);
        }

        private async void onListItemChosen(int id)
        {
            await LoadFriendAsync(id);
        }

        public async Task LoadFriendAsync(int friendId)
        {
            Friend = await _friendDataService.GetFriendAsync(friendId);
        }

        public Friend Friend
        {
            get { return _friend; }
            set
            {
                _friend = value;
                // przy każdym setowaniu odpalać ten event aby UI się updateowało
                OnProperyChanged(nameof(Friend));
            }
        }
    }
}
