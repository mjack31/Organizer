using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Organizer.DataAccess;
using Organizer.Models;

namespace Organizer.UI.Data
{
    public class LookupItemsDataService : ILookupItemsDataService, IProgLangLookupItemsDataService
    {
        private Func<OrganizerDbContext> _dbContextCreator;

        // nie można wstrzyknąć instancji dbContext poniważ chcemy tworzy instancje dynamicznie wewnątrz klasy. Każde GetAll musi tworzyć nowy kontekst. Przy zwykłym DI context był by stworzony tylko raz.
        // trzeba użyć dynamicznego operatora Func<>
        public LookupItemsDataService(Func<OrganizerDbContext> dbContextCreator)
        {
            _dbContextCreator = dbContextCreator;
        }

        public async Task<List<ListItem>> GetAllFriendsAsync()
        {
            using (var context = _dbContextCreator())
            {
                var listItem = await context.Friends.AsNoTracking().Select(f => new ListItem
                {
                    Id = f.Id,
                    Name = f.FirstName + " " + f.LastName
                }).ToListAsync();
                return listItem;
            }
        }

        public async Task<List<ListItem>> GetAllMeetingsAsync()
        {
            using (var context = _dbContextCreator())
            {
                var listItem = await context.Meetings.AsNoTracking().Select(f => new ListItem
                {
                    Id = f.Id,
                    Name = f.Title,
                }).ToListAsync();
                return listItem;
            }
        }

        public async Task<List<ListItem>> GetAllProgLangAsync()
        {
            using (var context = _dbContextCreator())
            {
                var listItem = await context.ProgramingLanguages.AsNoTracking().Select(f => new ListItem
                {
                    Id = f.Id,
                    Name = f.LanguageName
                }).ToListAsync();
                return listItem;
            }
        }
    }
}
