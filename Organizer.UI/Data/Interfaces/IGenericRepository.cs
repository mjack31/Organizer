﻿using Organizer.Models;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity> GetAsync(int id);
        Task SaveAsync();
        bool HasChanges();
        TEntity Add(TEntity model);
        void Delete(TEntity model);
    }
}