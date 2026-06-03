using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {

        private  GymDbContext gymDbContext;

        public PlanRepository(GymDbContext _gymDbContext)
        {
            gymDbContext = _gymDbContext;
        }


        #region Implement All CRUD Methods in Interface of IPlanRepository

        public async Task<IEnumerable<Plan>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var plans = isTracked ? gymDbContext.Plans : gymDbContext.Plans.AsNoTracking(); 

            return await plans.ToListAsync();
        }

        public async Task<Plan?> GetById(int id, CancellationToken ct = default)
        {
            return await gymDbContext.Plans.FirstOrDefaultAsync(plan => plan.Id == id);
        }

        public void Add(Plan plan)
        {
            gymDbContext.Plans.Add(plan);
        }

        public void Update(Plan plan)
        {
            gymDbContext.Plans.Update(plan);
        }
        public void Delete(int id)
        {
            var plan = gymDbContext.Plans.FirstOrDefault(plan => plan.Id == id);

            if (plan != null)
                gymDbContext.Remove(plan);
        }
        public async Task<int> CompleteAsync()
        {
            return await gymDbContext.SaveChangesAsync();
        }
        #endregion
    }
}
