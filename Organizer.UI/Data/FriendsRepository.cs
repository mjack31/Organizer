using Organizer.DataAccess;
using Organizer.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public class FriendsRepository : GenericRepository<Friend, OrganizerDbContext>, IFriendsRepository<Friend>
    {
        public FriendsRepository(OrganizerDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Friend> GetAsync(int id)
        {
            var friend = await _dbContext.Friends.Include("PhoneNumbers").Where(f => f.Id == id).FirstOrDefaultAsync();
            return friend;
        }

        public void RemovePhoneNumber(PhoneNumber numberToDel)
        {
            _dbContext.PhoneNumbers.Remove(numberToDel);
        }
    }
}
