using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Data.Contexts;
using GymSystemMVC.DAL.Models;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext gymDbContext;
        private readonly Dictionary<string, object> _Repos = [];



        #region Specific Repo With Specific Function
        public ISessionRepository SessionRepository { get; }
        public IMemberShipRepository MemberShipRepository { get; }

        public IBookingRepository BookingRepository { get; }
        #endregion
        public UnitOfWork
            (GymDbContext gymDbContext, 
             ISessionRepository sessionRepository,
             IMemberShipRepository memberShipRepository, 
             IBookingRepository bookingRepository)
        {
            this.gymDbContext = gymDbContext;
            SessionRepository = sessionRepository;
            MemberShipRepository = memberShipRepository;
            BookingRepository = bookingRepository;
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
