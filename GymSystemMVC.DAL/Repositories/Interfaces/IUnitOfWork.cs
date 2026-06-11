using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Entities;

namespace GymSystemMVC.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // Ex:
        // service => uniteOfWork.GetRepository<Trainer>().GetAll();
        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        public Task<int> CompleteAsync();
    }
}
