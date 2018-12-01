using System.Collections.Generic;
using Organizer.Models;

namespace Organizer.UI.Data
{
    public interface IFriendsDataService
    {
        IEnumerable<Friend> GetAll();
    }
}