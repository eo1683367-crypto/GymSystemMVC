using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemMVC.DAL.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }

        public ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
