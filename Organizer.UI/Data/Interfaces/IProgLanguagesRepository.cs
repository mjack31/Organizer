using Organizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public interface IProgLanguagesRepository<TEntity> : IGenericRepository<TEntity>
    {
        Task<List<Friend>> GetAllFriends();
    }
}