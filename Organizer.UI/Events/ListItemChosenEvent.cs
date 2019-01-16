using Prism.Events;

namespace Organizer.UI.Events
{
    public class ListItemChosenEvent : PubSubEvent<ListItemChosenEventArgs>
    {
    }

    public class ListItemChosenEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
