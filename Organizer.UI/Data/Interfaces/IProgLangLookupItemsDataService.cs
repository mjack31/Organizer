using System.Collections.Generic;
using System.Threading.Tasks;
using Organizer.Models;

namespace Organizer.UI.Data
{
    public interface IProgLangLookupItemsDataService
    {
        Task<List<ListItem>> GetAllProgLangAsync();
    }
}