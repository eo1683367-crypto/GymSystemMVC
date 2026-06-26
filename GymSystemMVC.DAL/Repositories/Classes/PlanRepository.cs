using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Data.Contexts;
using GymSystemMVC.DAL.Models;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class PlanRepository : GenaricRepository<Plan>,IPlanRepository
    {

        private  GymDbContext gymDbContext;

        public PlanRepository(GymDbContext _gymDbContext) : base(_gymDbContext)
        {
            gymDbContext = _gymDbContext;
        }

        // Implement any additional methods specific to MemberRepository if needed


    }
}
