using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymSystemMVC.DAL.Data.Contexts;
using GymSystemMVC.DAL.Models;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext gymDbContext;
        private readonly DbSet<TEntity> _set;

        public GenaricRepository(GymDbContext _gymDbContext)
        {
            gymDbContext = _gymDbContext;
            _set = gymDbContext.Set<TEntity>();
        }


        #region Implementing All Basic Signature of CRUD Operations
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool isTracked = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = isTracked ? _set : _set.AsNoTracking();

            if(predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }
        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default) => await _set.FindAsync([id],ct);
        

        public void Add(TEntity entity) => _set.Add(entity);
        
        public void Update(TEntity entity) => _set.Update(entity);
        
        public void Delete(int id)
        {
            var entity = _set.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                _set.Remove(entity);
            }
        }

        public void Delete(TEntity entity) => _set.Remove(entity);

        public Task<int> CompleteAsync() => gymDbContext.SaveChangesAsync();
        

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            var entity = isTracked ? _set : _set.AsNoTracking();

            return await entity.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _set.AnyAsync(predicate, ct);
        

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
              => predicate is null ? _set.AsNoTracking().CountAsync(ct) : _set.AsNoTracking().CountAsync(predicate, ct);
        
        #endregion
    }
}
