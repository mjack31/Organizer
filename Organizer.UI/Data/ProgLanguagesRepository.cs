using Organizer.DataAccess;
using Organizer.Models;

namespace Organizer.UI.Data
{
    public class ProgLanguagesRepository : GenericRepository<ProgramingLang, OrganizerDbContext>, IProgLanguagesRepository<ProgramingLang>
    {
        public ProgLanguagesRepository(OrganizerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
