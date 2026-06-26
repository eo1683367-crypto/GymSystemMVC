using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.AnalyticsViewModels;
using GymSystemMVC.DAL.Models;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnalyticsService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var sessions = await unitOfWork.GetRepository<Session>().GetAllAsync();

            var totalMember = await unitOfWork.GetRepository<Member>().CountAsync(ct:ct);
            var totalTrainers = await unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
            var activeMembers = await unitOfWork.GetRepository<MemberShip>().CountAsync(m=> m.EndDate > DateTime.Now,ct);

            return new AnalyticsViewModel
            {
                TotalMembers = totalMember,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = sessions.Count(s => s.EndDate <  DateTime.Now),
            };

        }
    }
}
