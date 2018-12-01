using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public class FriendsDummyDataService : IFriendsDataService
    {
        // TODO - Dodać pobieranie danych z DB
        public IEnumerable<Friend> GetAll()
        {
            List<Friend> friends = new List<Friend>();
            friends.Add(new Friend { FirstName = "Joe", LastName = "Stone" });
            friends.Add(new Friend { FirstName = "Karl", LastName = "Kek" });
            friends.Add(new Friend { FirstName = "Anna", LastName = "Klu" });
            return friends;
        }
    }
}
