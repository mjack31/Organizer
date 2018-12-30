using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public interface IFriendDetailsViewModel
    {
        bool HasChanges { get; set; }
        Task LoadFriendAsync(int? friendId);
    }
}