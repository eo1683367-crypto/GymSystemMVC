using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext gymDbContext;

        public GenaricRepository(GymDbContext _gymDbContext)
        {
            gymDbContext = _gymDbContext;
        }


        #region Implementing All Basic Signature of CRUD Operations
        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var entity = isTracked ? gymDbContext.Set<TEntity>() : gymDbContext.Set<TEntity>().AsNoTracking();

            return await entity.ToListAsync();
        }
        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
        {
            return await gymDbContext.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Add(TEntity entity)
        {
            gymDbContext.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            gymDbContext.Set<TEntity>().Update(entity);
        }
        public void Delete(int id)
        {
            var entity = gymDbContext.Set<TEntity>().FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                gymDbContext.Set<TEntity>().Remove(entity);
            }
        }

        public Task<int> CompleteAsync()
        {
            return gymDbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            var entity = isTracked ? gymDbContext.Set<TEntity>() : gymDbContext.Set<TEntity>().AsNoTracking();

            return await entity.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
           return await gymDbContext.Set<TEntity>().AnyAsync(predicate, ct);
        }
        #endregion
    }
}
