using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using GymSystemMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystemMVC.DAL.Data.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50)
                                         .IsRequired();

            builder.Property(p => p.Description).HasMaxLength(200)
                                         .IsRequired();

            builder.Property(p => p.Price).HasPrecision(10, 2);  // decimal(10,2)

            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GetDate()");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("DurationCheckValue", "Duration Between 1 and 365");
            });
        }
    }
}
