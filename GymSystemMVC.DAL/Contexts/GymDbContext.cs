using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Contexts
{
    public class GymDbContext : DbContext
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
            // السطر الواحد ده بيلاقي كل الـ Configuration Classes 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);

        }
    }
}
