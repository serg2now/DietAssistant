using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;

namespace DietAssistant.Services.Interfaces
{
    public interface IAdminReportService : IReportService
    {
        Task<IEnumerable<GroupStatistic>> GetUsersDailyStatisticAsync(DateTime date);
    }
}
