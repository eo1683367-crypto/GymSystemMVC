using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext gymDbContext;
        private readonly Dictionary<string, object> _Repos = [];

        public UnitOfWork(GymDbContext gymDbContext)
        {
            this.gymDbContext = gymDbContext;
        }
      

        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name; // string Key 


            // Check to prevent creating multiple repositories of the same type in the same request
            if (_Repos.TryGetValue(typeName, out object oldRepo))
            {
                return (IGenaricRepository<TEntity>) oldRepo;
            }

            var newRepo = new GenaricRepository<TEntity>(gymDbContext);

            _Repos[typeName] = newRepo;
            return newRepo;
        }

        public async Task<int> CompleteAsync()
        {
            return await gymDbContext.SaveChangesAsync();
        }
    }
}
