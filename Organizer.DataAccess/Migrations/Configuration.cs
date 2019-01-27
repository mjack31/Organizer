namespace Organizer.DataAccess.Migrations
{
    using Organizer.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Organizer.DataAccess.OrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OrganizerDbContext context)
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

            // zapewnienie ¿e przed seedowaniem numerów telefonów s¹ friendsy
            context.SaveChanges();

            context.PhoneNumbers.AddOrUpdate(f => f.Number, 
                new PhoneNumber { Number = "609696223", FriendId = context.Friends.FirstOrDefault().Id }
            );

            context.Meetings.AddOrUpdate(f => f.Title,
                new Meeting { Title = "First", FromDate = new DateTime(2020, 3, 3), ToDate = new DateTime(2020, 3, 23), Friends = new List<Friend>
                {
                    context.Friends.FirstOrDefault(f => f.FirstName == "Jacek")
                }
            });
        }
    }
}
