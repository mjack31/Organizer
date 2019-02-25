using Organizer.DataAccess;
using Organizer.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public class ProgLanguagesRepository : GenericRepository<ProgramingLang, OrganizerDbContext>, IProgLanguagesRepository<ProgramingLang>
    {
        public ProgLanguagesRepository(OrganizerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Friend>> GetAllFriends()
        {
            return await _dbContext.Set<Friend>().ToListAsync();
        }
    }
}
