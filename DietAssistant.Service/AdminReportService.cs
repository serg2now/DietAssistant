using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Interfaces;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Interfaces;

namespace DietAssistant.Services
{
    public class AdminReportService : ReportService, IAdminReportService
    {
        public AdminReportService(
            IRepository<DailyReport> reportRepository,
            IUserRepository userRepository,
            IRepository<ConsumedDish> consumedDishRepository,
            IParametersCalculationService calculationService,
            IDietService dietService,
            IMapper mapper)
            :base(reportRepository, userRepository, consumedDishRepository, calculationService, dietService, mapper)
        {
        }

        public async Task<IEnumerable<GroupStatistic>> GetUsersDailyStatisticAsync(DateTime date)
        {
            var reports = await _reportRepository.GetItemsAsync(r => r.ReportDate == date, null, "User");
            var reportsDto = _mapper.Map<IEnumerable<AdminReportDTO>>(reports);

            var groupedReports = reportsDto.GroupBy(r => r.BodyType, (key, reports) 
                => new GroupStatistic 
                { 
                    Key = key, 
                    KeyDisplayValue = reports.First().BodyTypeDisplayValue,
                    Values = reports 
                }).ToArray();

            _calculationService.CalculateGroupsAverages(groupedReports);

            return groupedReports;
        }
    }
}
