using Organizer.UI.ViewModels;
using Prism.Events;

namespace Organizer.UI.Events
{
    public class TabClosedEvent : PubSubEvent<TabClosedEventArgs>
    {
    }

    public class TabClosedEventArgs
    {
        public IDetailsViewModel DetailViewModel { get; set; }
    }
}
