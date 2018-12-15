using Organizer.DataAccess;
using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public class FriendDetailsDataService : IFriendDetailsDataService
    {
        private Func<OrganizerDbContext> _dbContextCreator;

        public FriendDetailsDataService(Func<OrganizerDbContext> dbContextCreator)
        {
            _dbContextCreator = dbContextCreator;
        }

        public async Task<Friend> GetFriendAsync(int id)
        {
            using (var context = _dbContextCreator())
            {
                var friend =  await context.Friends.AsNoTracking().Where(f => f.Id == id).FirstOrDefaultAsync();
                return friend;
            }
        }

        public async Task SaveFriendAsync(Friend friend)
        {
            using (var context = _dbContextCreator())
            {
                context.Friends.Attach(friend);
                context.Entry(friend).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
