using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Models
{
    public class Meeting
    {
        public Meeting()
        {
            Friends = new List<Friend>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<Friend> Friends { get; set; }
    }
}
