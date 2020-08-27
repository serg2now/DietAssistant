using System.Collections.Generic;
using System.Linq;
using DietAssistant.DAL.Models;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Interfaces;

namespace DietAssistant.Services
{
    public class ParametersCalculationService : IParametersCalculationService
    {
        public virtual void CalculateDailyAmount(IEnumerable<ConsumedDish> consumedDishes, DailyReport report)
        {
            report.CarbohydratesAmount = 0;
            report.ProteinsAmount = 0;
            report.FatsAmount = 0;

            foreach(var dish in consumedDishes)
            {
                report.CarbohydratesAmount += dish.CarbohydratesAmount;
                report.ProteinsAmount += dish.ProteinsAmount;
                report.FatsAmount += dish.FatsAmount;
            }
        }

        public virtual void CalculateGroupsAverages(IEnumerable<GroupStatistic> groupStatistics)
        {
            foreach(var group in groupStatistics)
            {
                decimal sumProteinsAmount = 0;
                decimal sumCarbohydratesAmount = 0;
                decimal sumFatsAmount = 0;
                int reportsCount = group.Values.Count();

                foreach(var report in group.Values)
                {
                    sumCarbohydratesAmount += report.CarbohydratesAmount;
                    sumProteinsAmount += report.ProteinsAmount;
                    sumFatsAmount += report.FatsAmount;
                }

                group.AverageCarbohydratesAmount = sumCarbohydratesAmount / reportsCount;
                group.AverageProteinsAmount = sumProteinsAmount / reportsCount;
                group.AverageFatsAmount = sumFatsAmount / reportsCount;
            }
        }
    }
}
