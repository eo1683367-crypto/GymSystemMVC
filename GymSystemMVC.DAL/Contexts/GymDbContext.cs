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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // configure the connection string to your database
            optionsBuilder.UseSqlServer("Server=.; Database= GymSystemMVCDb ; trusted_connection= True; trustservercertificate= True;");
        }
        #endregion


        // Define your DbSet properties for each entity here
        #region My DbSets
        public DbSet<Plan> Plans { get; set; }


        #endregion


        //---------------------------------------------------------------------------------------------------------------------------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // السطر الواحد ده بيلاقي كل الـ Configuration Classes 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);

        }
    }
}
