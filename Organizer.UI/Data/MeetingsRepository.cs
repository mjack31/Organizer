using Organizer.DataAccess;
using Organizer.Models;
using Organizer.UI.Data.Interfaces;

namespace Organizer.UI.Data
{
    public class MeetingsRepository : GenericRepository<Meeting, OrganizerDbContext>, IMeetingsRepository<Meeting>
    {
        public MeetingsRepository(OrganizerDbContext dbContext) : base(dbContext)
        {

        }
    }
}
