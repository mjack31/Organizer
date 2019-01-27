using Organizer.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Organizer.DataAccess
{
    public class OrganizerDbContext : DbContext
    {
        // dziedziczenie bazowego kontruktora tylko po to aby nadać nazwę bazie danych. Kontruktor jako parametr przyjmuje właśnie nazwę
        public OrganizerDbContext() : base("OrganizerDb")
        {
        }

        // lista tablic
        public DbSet<Friend> Friends { get; set; }
        public DbSet<ProgramingLang> ProgramingLanguages { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // usunięcie tworzenia tabel z nazwami w liczbie mnogiej
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
