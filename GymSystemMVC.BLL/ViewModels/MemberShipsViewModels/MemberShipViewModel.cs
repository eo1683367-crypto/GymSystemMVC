using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper.Execution;

namespace GymSystemMVC.BLL.ViewModels.MemberShipsViewModels
{
    public class MemberShipViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = default!;

        public int PlanId { get; set; }
        public string PlanName { get; set; } = default!;
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
