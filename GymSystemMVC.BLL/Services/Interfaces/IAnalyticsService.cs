using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.ViewModels.AnalyticsViewModels;

namespace GymSystemMVC.BLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default);
    }
}
