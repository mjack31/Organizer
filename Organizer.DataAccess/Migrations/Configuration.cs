namespace Organizer.DataAccess.Migrations
{
    using Organizer.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Organizer.DataAccess.OrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Organizer.DataAccess.OrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(f => f.FirstName,
                new Friend { FirstName = "Joe", LastName = "Stone" },
                new Friend { FirstName = "Karl", LastName = "Kek" },
                new Friend { FirstName = "Anna", LastName = "Klu" }
            );

            context.ProgramingLanguages.AddOrUpdate(f => f.LanguageName,
                new ProgramingLang { LanguageName = "C#" },
                new ProgramingLang { LanguageName = "JavaScript" },
                new ProgramingLang { LanguageName = "Cobol" },
                new ProgramingLang { LanguageName = "Java" }
            );
        }
    }
}
