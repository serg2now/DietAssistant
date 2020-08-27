using DietAssistant.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DietAssistant.Services.Interfaces
{
    public interface IReportService
    {
        Task<DailyReportDTO> UpsertDailyReportAsync(int customerId, DateTime reportDate);

        Task<DailyReportDTO> GetDailyReportAsync(int customerId, DateTime reportDate);

        Task<IEnumerable<DailyReportDTO>> GetAllReportsAsync();

        Task<IEnumerable<DailyReportDTO>> GetReportsByDateAsync(DateTime reportDate);
    }
}
