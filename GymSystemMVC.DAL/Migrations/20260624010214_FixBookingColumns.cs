using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystemMVC.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "Bookings",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Bookings",
                newName: "BookingDate");
        }
    }
}