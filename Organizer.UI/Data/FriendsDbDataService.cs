using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Organizer.DataAccess;
using Organizer.Models;

namespace Organizer.UI.Data
{
    class FriendsDbDataService : IFriendsDataService
    {
        private Func<OrganizerDbContext> _dbContextCreator;

        // nie można wstrzyknąć instancji dbContext poniważ chcemy tworzy instancje dynamicznie wewnątrz klasy. Każde GetAll musi tworzyć nowy kontekst. Przy zwykłym DI kontext był by stworzony tylko raz.
        // trzeba użyć dynamicznego operatora Func<>
        public FriendsDbDataService(Func<OrganizerDbContext> dbContextCreator)
        {
            _dbContextCreator = dbContextCreator;
        }

        public IEnumerable<Friend> GetAll()
        {
            using (var context = _dbContextCreator())
            {
                return context.Friends.ToList();
            }
        }
    }
}
