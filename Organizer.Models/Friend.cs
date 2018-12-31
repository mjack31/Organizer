using System.ComponentModel.DataAnnotations;

namespace Organizer.Models
{
    public class Friend
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int? FavoriteLanguageId { get; set; }

        public ProgramingLang FavoriteLanguage { get; set; }
    }
}
