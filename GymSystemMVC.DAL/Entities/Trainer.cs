using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Entities.Enums;

namespace GymSystemMVC.DAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialites Specialize { get; set; }
        public DateTime HiringDate { get; set; }

        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
