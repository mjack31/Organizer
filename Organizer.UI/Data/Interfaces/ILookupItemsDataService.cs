using System.Collections.Generic;
using System.Threading.Tasks;
using Organizer.Models;

namespace Organizer.UI.Data
{
    public interface ILookupItemsDataService
    {
        Task<List<ListItem>> GetAllFriendsAsync();
        Task<List<ListItem>> GetAllMeetingsAsync();
    }
}