using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Data.Contexts;
using GymSystemMVC.DAL.Models;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class MemberRepository : GenaricRepository<Member>,IMemberRepository
    {
        private readonly GymDbContext gymDbContext;

        public MemberRepository(GymDbContext _gymDbContext) : base(_gymDbContext) 
        {
            gymDbContext = _gymDbContext;
        }

        // Implement any additional methods specific to MemberRepository if needed

    }
}
