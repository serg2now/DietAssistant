using DietAssistant.Services;
using DietAssistant.Services.Interfaces;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using System;
using DietAssistant.DAL.Models;
using DietAssistant.Services.DTOs;

namespace DietAssistant.Tests
{
    public class DietParametersServiceTests
    {
        private IDietService _dietService;

        public DietParametersServiceTests()
        {
            var limits = new DietLimits
            {
                CarbohydratesLimits = new Limits { Min = 100, Max = 300 },
                ProteinsLimits = new Limits { Min = 100, Max = 200 },
                FatsLimits = new Limits { Min = 100, Max = 200 }
            };

            _dietService = new DietParametersService(limits);
        }

        [Fact]
        public void InitAndValidateLimits_IfFoodLimitsAreNotSet_ThrowsException()
        {
            //Prepare test
            var limits = new DietLimits();

            //Do test
            var exception = Assert.Throws<ArgumentException>(() => new DietParametersService(limits));
            Assert.Equal("All Food limits should be defined in appsettings.json file", exception.Message);
        }

        [Fact]
        public void ValidateDailyReport()
        {
            //Prepare test
            var report = new DailyReport
            {
                CarbohydratesAmount = 200,
                ProteinsAmount = 150,
                FatsAmount = 170
            };

            //Do test 
            _dietService.ValidateDailyReport(report);

            //Assert
            Assert.False(report.HasWarnings);
            Assert.Equal(0, report.Warnings.Length);
        }

        [Fact]
        public void ValidateDailyReport_WhenParametersDontMatchesLimits_ReturnsErrorInWarnings() 
        {
            //Prepare test
            var report = new DailyReport
            {
                CarbohydratesAmount = 310,
                ProteinsAmount = 250,
                FatsAmount = 50
            };

            //Do test
            _dietService.ValidateDailyReport(report);

            //Assert
            Assert.True(report.HasWarnings);
            Assert.Contains("Proteins amount is greather", report.Warnings);
            Assert.Contains("Carbohydrates amount is greather", report.Warnings);
            Assert.Contains("Fats amount is lower", report.Warnings);
        }
    }
}
