using Organizer.Models;
using Organizer.UI.Data;
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

        public FriendDetailsViewModel(IFriendDetailsDataService friendDataService)
        {
            _friendDataService = friendDataService;
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
