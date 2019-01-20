using Organizer.Models;

namespace Organizer.UI.Data
{
    public interface IFriendsRepository<TEntity> : IGenericRepository<TEntity>
    {
        void RemovePhoneNumber(PhoneNumber numberToDel);
    }
}