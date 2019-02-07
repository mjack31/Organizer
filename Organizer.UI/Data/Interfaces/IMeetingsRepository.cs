using Organizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organizer.UI.Data.Interfaces
{
    public interface IMeetingsRepository<TEntity> : IGenericRepository<TEntity>
    {
        Task<List<Friend>> GetAllAddedFriends();
    }
}
