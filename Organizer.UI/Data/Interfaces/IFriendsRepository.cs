using Organizer.Models;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public interface IFriendsRepository
    {
        Task<Friend> GetFriendAsync(int id);
        Task SaveFriendAsync();
        bool HasChanges();
        Friend Add(Friend model);
        void Delete(Friend model);
    }
}