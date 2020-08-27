using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DietAssistant.Services.DTOs;

namespace DietAssistant.Services.Interfaces
{
    public interface IConsumedDishService
    {
        bool AutoUpdateDailyReport { get; set; }

        Task<IEnumerable<ConsumeLogItem>> GetLogItemsAsync(int customerId, DateTime consumeDate);

        Task<int> AddLogItemAsync(ConsumeLogItem logItem);

        Task<int> UpdateLogItem(ConsumeLogItem logItem);
    }
}
