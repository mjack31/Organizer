using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public interface IDetailsViewModel
    {
        bool HasChanges { get; set; }
        Task LoadFriendAsync(int? id);
    }
}