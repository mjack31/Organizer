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
    public class FriendsRepository : IFriendsRepository
    {
        private OrganizerDbContext _dbContext;

        public FriendsRepository(OrganizerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Friend> GetFriendAsync(int id)
        {
            var friend = await _dbContext.Friends.Where(f => f.Id == id).FirstOrDefaultAsync();
            return friend;
        }

        public bool HasChanges()
        {
            return _dbContext.ChangeTracker.HasChanges();
        }

        public async Task SaveFriendAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
