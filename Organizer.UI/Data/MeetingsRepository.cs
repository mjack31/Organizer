using System.Threading.Tasks;
using Organizer.DataAccess;
using Organizer.Models;
using Organizer.UI.Data.Interfaces;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace Organizer.UI.Data
{
    public class MeetingsRepository : GenericRepository<Meeting, OrganizerDbContext>, IMeetingsRepository<Meeting>
    {
        public MeetingsRepository(OrganizerDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Meeting> GetAsync(int id)
        {
            var meeting = await _dbContext.Meetings.Include("Friends").Where(f => f.Id == id).FirstOrDefaultAsync();
            return meeting;
        }

        public async Task<List<Friend>> GetAllAddedFriends()
        {
            return await _dbContext.Set<Friend>().ToListAsync();
        }
    }
}
