namespace Organizer.Models
{
    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // dodana klasa dziedzicząca po ListItem po to aby w łatwy sposób można było stworzyć obiekt nullable bez przerabiania reszty programu
    public class NullListItem : ListItem
    {
        public new int? Id
        {
            get { return null; }
        }
    }
}
