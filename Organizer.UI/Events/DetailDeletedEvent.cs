using Prism.Events;

namespace Organizer.UI.Events
{
    public class DetailDeletedEvent : PubSubEvent<DetailDeletedEventArgs>
    {
    }

    public class DetailDeletedEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
