using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymSystemMVC.DAL.Entities;

namespace GymSystemMVC.DAL.Repositories.Interfaces
{
    public interface IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
    {
        // All Basic Signature of CRUD Operations

        Task<IEnumerable<TEntity>> GetAll(bool isTracked = false, CancellationToken ct = default);
        Task<TEntity?> GetById(int id, CancellationToken ct = default);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        Task<int> CompleteAsync();
        //----------------------------------------------------------------------------------------------

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,bool isTracked = false, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,  CancellationToken ct = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default);
    }
}
