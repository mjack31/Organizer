using System.Threading.Tasks;

namespace Organizer.UI.ViewModels
{
    public interface IDetailsViewModel
    {
        bool HasChanges { get; set; }
        Task LoadDetailAsync(int? id);
        int Id { get; }
    }
}