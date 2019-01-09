﻿using Organizer.DataAccess;
using Organizer.Models;
using System.Data.Entity;
using System.Linq;
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

        public Friend Add(Friend model)
        {
            var friend = _dbContext.Friends.Add(model);
            return friend;
        }

        public void Delete(Friend model)
        {
            _dbContext.Friends.Remove(model);
            _dbContext.SaveChanges();
        }

        public async Task<Friend> GetFriendAsync(int id)
        {
            var friend = await _dbContext.Friends.Include("PhoneNumbers").Where(f => f.Id == id).FirstOrDefaultAsync();
            return friend;
        }

        public bool HasChanges()
        {
            return _dbContext.ChangeTracker.HasChanges();
        }

        public void RemovePhoneNumber(PhoneNumber numberToDel)
        {
            _dbContext.PhoneNumbers.Remove(numberToDel);
        }

        public async Task SaveFriendAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}