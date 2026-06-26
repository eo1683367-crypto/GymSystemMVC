using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Data.Contexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        // Configure the database connection string and other options here
        #region Configure Connection String
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // configure the connection string to your database
        //    optionsBuilder.UseSqlServer("Server=.; Database= GymSystemMVCDb ; trusted_connection= True; trustservercertificate= True;");
        //}

        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {   
        }
        #endregion


        // Define your DbSet properties for each entity here
        #region My DbSets

     
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MemberShip> Memberships { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        #endregion


        //---------------------------------------------------------------------------------------------------------------------------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder); // this for apply Identity Configuration 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly); // app Configuration

        }
    }
}
