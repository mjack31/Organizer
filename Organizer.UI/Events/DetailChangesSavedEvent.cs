using Organizer.Models;
using Prism.Events;

namespace Organizer.UI.Events
{
    public class DetailChangesSavedEvent : PubSubEvent<DetailChangesSavedEventArgs>
    {
    }

    public class DetailChangesSavedEventArgs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ViewModelName { get; set; }
    }
}
