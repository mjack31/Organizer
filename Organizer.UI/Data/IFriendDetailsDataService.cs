﻿using Organizer.Models;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public interface IFriendDetailsDataService
    {
        Task<Friend> GetFriendAsync(int id);
        Task SaveFriendAsync(Friend friend);
    }
}