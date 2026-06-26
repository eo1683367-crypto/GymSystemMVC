using GymSystemMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystemMVC.DAL.Data.Configurations
{
    internal class BookingConfigurations : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(X => X.Id);
            builder.Property(X => X.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(X => X.Session)
                   .WithMany(X => X.Bookings)
                   .HasForeignKey(X => X.SessionId);

            builder.HasOne(X => X.Member)
                   .WithMany(X => X.Bookings)
                   .HasForeignKey(X => X.MemberId);

            builder.HasKey(X => new { X.SessionId, X.MemberId });
        }
    }
}
