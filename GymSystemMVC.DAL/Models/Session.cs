using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemMVC.DAL.Models
{
    public class Session :BaseEntity
    {
        public string Description { get; set; }
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Trainer Trainer { get; set; }
        public int TrainerId { get; set; }

        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();


    }
}
