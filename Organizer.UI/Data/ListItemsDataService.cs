using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Organizer.DataAccess;
using Organizer.Models;

namespace Organizer.UI.Data
{
    public class ListItemsDataService : IListItemsDataService
    {
        private Func<OrganizerDbContext> _dbContextCreator;

        // nie można wstrzyknąć instancji dbContext poniważ chcemy tworzy instancje dynamicznie wewnątrz klasy. Każde GetAll musi tworzyć nowy kontekst. Przy zwykłym DI kontext był by stworzony tylko raz.
        // trzeba użyć dynamicznego operatora Func<>
        public ListItemsDataService(Func<OrganizerDbContext> dbContextCreator)
        {
            _dbContextCreator = dbContextCreator;
        }

        public async Task<List<ListItem>> GetAllAsync()
        {
            using (var context = _dbContextCreator())
            {
                var listItem = await context.Friends.AsNoTracking().Select(f => new ListItem
                {
                    Id = f.Id,
                    Name = f.FirstName + " " + f.LastName
                }).ToListAsync();
                //await Task.Delay(5000); // dla testów responsywności przy ładowaniu danych
                return listItem;
            }
        }
    }
}
