using DietAssistant.DAL.Models;
using DietAssistant.Services.DTOs;
using System.Collections.Generic;

namespace DietAssistant.Services.Interfaces
{
    public interface IParametersCalculationService
    {
        void CalculateDailyAmount(IEnumerable<ConsumedDish> consumedDishes, DailyReport report);

        void CalculateGroupsAverages(IEnumerable<GroupStatistic> groupStatistics);
    }
}
