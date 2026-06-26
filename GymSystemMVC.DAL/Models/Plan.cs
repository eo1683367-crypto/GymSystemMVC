using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemMVC.DAL.Models
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }

        public ICollection<MemberShip> MemberShips { get; set; } = new HashSet<MemberShip>();

    }
}
