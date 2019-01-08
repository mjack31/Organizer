using System.ComponentModel.DataAnnotations;

namespace Organizer.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{3})$", ErrorMessage = "Numer ma nieprawidłowy format")]
        [Required]
        public string Number { get; set; }

        public int FriendId { get; set; }

        public Friend Friend { get; set; }
    }
}
