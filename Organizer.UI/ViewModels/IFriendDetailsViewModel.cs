using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public interface IFriendDetailsViewModel
    {
        Task LoadFriendAsync(int friendId);
    }
}