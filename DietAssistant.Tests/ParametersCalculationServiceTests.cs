using DietAssistant.DAL.Models;
using DietAssistant.Services;
using DietAssistant.Services.DTOs;
using System.Collections.Generic;
using Xunit;

namespace DietAssistant.Tests
{
    public class ParametersCalculationServiceTests
    {
        [Fact]
        public void CalculateDailyAmount()
        {
            //Prepare test 
            var calculationService = new ParametersCalculationService();
            var report = new DailyReport();
            var consumedDishes = new List<ConsumedDish>
            {
                new ConsumedDish
                {
                    CarbohydratesAmount = 50,
                    FatsAmount = 0,
                    ProteinsAmount = 100,
                },
                new ConsumedDish
                {
                    CarbohydratesAmount = 0,
                    FatsAmount = 200,
                    ProteinsAmount = 200
                }
            };

            //Do test
            calculationService.CalculateDailyAmount(consumedDishes, report);

            //Assert
            Assert.Equal(50, report.CarbohydratesAmount);
            Assert.Equal(200, report.FatsAmount);
            Assert.Equal(300, report.ProteinsAmount);
        }

        [Fact]
        public void CalculateGroupsAverages()
        {
            //Prepare test
            var calculationService = new ParametersCalculationService();

            var groupStatistic = new GroupStatistic
            {
                Values = new List<AdminReportDTO>
                {
                    new AdminReportDTO
                    {
                        CarbohydratesAmount = 300,
                        ProteinsAmount = 100,
                        FatsAmount = 250
                    },
                    new AdminReportDTO
                    {
                        CarbohydratesAmount = 100,
                        ProteinsAmount = 200,
                        FatsAmount = 350
                    }
                }
            };

            var groupedStatistic = new List<GroupStatistic> { groupStatistic };

            //Do test
            calculationService.CalculateGroupsAverages(groupedStatistic);

            //Assert
            Assert.Equal(200, groupedStatistic[0].AverageCarbohydratesAmount);
            Assert.Equal(150, groupedStatistic[0].AverageProteinsAmount);
            Assert.Equal(300, groupedStatistic[0].AverageFatsAmount);
        }
    }
}
