using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Models;

namespace GymSystemMVC.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // Ex:
        // service => uniteOfWork.GetRepository<Trainer>().GetAll();
        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        public Task<int> CompleteAsync();

        public ISessionRepository SessionRepository { get; }
        public IMemberShipRepository MemberShipRepository { get; }

        public IBookingRepository BookingRepository { get; }
    }
}
