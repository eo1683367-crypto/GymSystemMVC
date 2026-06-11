using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemMVC.DAL.Entities
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<MemberShip> MemberShips { get; set; } = new HashSet<MemberShip>();
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();


    }
}
