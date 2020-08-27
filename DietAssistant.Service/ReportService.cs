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
    public class ReportService : IReportService
    {
        protected readonly IRepository<DailyReport> _reportRepository;
        protected readonly IRepository<ConsumedDish> _consumedDishRepository;
        protected readonly IParametersCalculationService _calculationService;
        protected readonly IDietService _dietService;
        protected readonly IUserRepository _userRepository;
        protected readonly IMapper _mapper;

        public ReportService(
            IRepository<DailyReport> reportRepository, 
            IUserRepository userRepository,
            IRepository<ConsumedDish> consumedDishRepository,
            IParametersCalculationService calculationService,
            IDietService dietService, 
            IMapper mapper)
        {
            _reportRepository = reportRepository;
            _userRepository = userRepository;
            _consumedDishRepository = consumedDishRepository;
            _calculationService = calculationService;
            _dietService = dietService;
            _mapper = mapper;
        }

        public virtual async Task<DailyReportDTO> UpsertDailyReportAsync(int customerId, DateTime reportDate)
        {
            var consumedDishes = await _consumedDishRepository.GetItemsAsync(d => d.UserId == customerId && d.DateOfConsume == reportDate);
            var report = new DailyReportDTO() { ReportDate = reportDate };
            
            if (consumedDishes.Any())
            {
                var dbReport =
                    (await _reportRepository.GetItemsAsync(
                        i => i.UserId == customerId && i.ReportDate == reportDate, null, "User"))
                    .FirstOrDefault();

                dbReport ??= new DailyReport { UserId = customerId, ReportDate = reportDate };

                _calculationService.CalculateDailyAmount(consumedDishes, dbReport);
                _dietService.ValidateDailyReport(dbReport);

                var result = 0;

                if (dbReport.Id > 0)
                {
                    result = await _reportRepository.UpdateItemAsync(dbReport);
                }
                else
                {
                    result = await _reportRepository.AddItemAsync(dbReport);

                    var customer = (result > 0) ? await _userRepository.GetItemAsync(customerId) : null;
                    dbReport.User = (result > 0) ? new User() { Name = customer.Name, Surname = customer.Surname } : null;
                }

                if (result > 0 || dbReport.Id != 0)
                {
                    report = _mapper.Map<DailyReportDTO>(dbReport);
                }
            }
            else
            {
                report.HasWarnings = true;
                report.Warnings = "There are no consume data for daily report!";
            }

            return report;
        }

        public virtual async Task<IEnumerable<DailyReportDTO>> GetAllReportsAsync()
        {
            var items = await _reportRepository.GetItemsAsync(null, r => r.OrderBy(i => i.ReportDate), "User");

            return _mapper.Map<IEnumerable<DailyReportDTO>>(items);
        }

        public virtual async Task<DailyReportDTO> GetDailyReportAsync(int customerId, DateTime reportDate)
        {
            var items = (await _reportRepository.GetItemsAsync(
                r => r.UserId == customerId && r.ReportDate == reportDate, 
                r => r.OrderBy(i => i.ReportDate), "User")).FirstOrDefault();

            return _mapper.Map<DailyReportDTO>(items);
        }

        public virtual async Task<IEnumerable<DailyReportDTO>> GetReportsByDateAsync(DateTime reportDate)
        {
            var items = await _reportRepository.GetItemsAsync(
                r => r.ReportDate == reportDate,
                r => r.OrderBy(i => i.ReportDate), "User");

            return _mapper.Map<IEnumerable<DailyReportDTO>>(items);
        }
    }
}
