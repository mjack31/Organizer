using System.ComponentModel.DataAnnotations;

namespace Organizer.Models
{
    public class ProgramingLang
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LanguageName { get; set; }
    }
}
