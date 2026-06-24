using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemMVC.BLL.ViewModels.BookingViewModels
{
    public class MemberForSessionViewModel
    {
        public int MemberId { get; set; }

        public int SessionId { get; set; }

        public string MemberName { get; set; }

        public bool IsAttended { get; set; } = false;

        public DateTime BookingDate { get; set; }
    }
}
