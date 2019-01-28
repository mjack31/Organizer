using System.Threading.Tasks;
using Organizer.UI.Wrappers;

namespace Organizer.UI.ViewModels
{
    public interface IMeetingDetailsViewModel : IDetailsViewModel
    {
        MeetingWrapper Meeting { get; set; }
    }
}