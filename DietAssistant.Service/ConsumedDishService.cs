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
    public class ConsumedDishService : IConsumedDishService
    {
        protected readonly IRepository<ConsumedDish> _consumedDishRepository;
        protected readonly IReportService _reportService;
        protected readonly IMapper _mappper;

        public ConsumedDishService(IRepository<ConsumedDish> consumedDishRepository, IReportService reportService, IMapper mapper)
        {
            _consumedDishRepository = consumedDishRepository;
            _reportService = reportService;
            _mappper = mapper;
        }

        public bool AutoUpdateDailyReport { get; set; }

        public virtual async Task<int> AddLogItemAsync(ConsumeLogItem logItem)
        {
            var dbItem = _mappper.Map<ConsumedDish>(logItem);

            var result = await _consumedDishRepository.AddItemAsync(dbItem);

            if (AutoUpdateDailyReport)
            {
                await _reportService.UpsertDailyReportAsync(logItem.CustomerId, logItem.DateOfConsume);
            }

            return result;
        }

        public virtual async Task<IEnumerable<ConsumeLogItem>> GetLogItemsAsync(int customerId, DateTime consumeDate)
        {
            var logItems = await _consumedDishRepository.GetItemsAsync(
                d => d.UserId == customerId && d.DateOfConsume == consumeDate,
                q => q.OrderBy(d => d.ConsumeTimeTypeId));

            return _mappper.Map<IEnumerable<ConsumeLogItem>>(logItems);
        }

        public virtual async Task<int> UpdateLogItem(ConsumeLogItem logItem)
        {
            var consumedDish = await _consumedDishRepository.GetItemAsync(logItem.Id);

            if (consumedDish == null)
            {
                throw new ArgumentException($"Consumed dish with id {logItem.Id} does not exist in database!");
            }

            var dbItem = _mappper.Map<ConsumedDish>(logItem);

            var result = await _consumedDishRepository.UpdateItemAsync(dbItem);

            if (AutoUpdateDailyReport)
            {
                await _reportService.UpsertDailyReportAsync(logItem.CustomerId, logItem.DateOfConsume);
            }

            return result;
        }
    }
}
