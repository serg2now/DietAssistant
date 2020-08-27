using AutoMapper;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Interfaces;
using DietAssistant.Services;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;
using DietAssistant.Services.Interfaces;
using DietAssistant.Services.MappingConfigurations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DietAssistant.Tests
{
    public class AdminReportServiceTests
    {
        private Mock<IRepository<DailyReport>> reportsRepositoryMock;
        private IAdminReportService adminReportService;
        private Mock<IParametersCalculationService> calculationsServiceMock;

        public AdminReportServiceTests()
        {
            reportsRepositoryMock = new Mock<IRepository<DailyReport>>();

            var dishRepositoryMock = new Mock<IRepository<ConsumedDish>>();
            var usersRepositoryMock = new Mock<IUserRepository>();
            calculationsServiceMock = new Mock<IParametersCalculationService>();
            var dietServiceMock = new Mock<IDietService>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new DailyReportMappingProfile()));
            var mapper = new Mapper(configuration);

            adminReportService = new AdminReportService(
                reportsRepositoryMock.Object,
                usersRepositoryMock.Object,
                dishRepositoryMock.Object,
                calculationsServiceMock.Object,
                dietServiceMock.Object,
                mapper);
        }

        [Fact]
        public async Task GetUsersDailyStatisticAsync()
        {
            //Prepare test
            reportsRepositoryMock
                .Setup(r => r.GetItemsAsync(
                    It.IsAny<Expression<Func<DailyReport, bool>>>(),
                    null,
                    "User"))
                .ReturnsAsync(new List<DailyReport> 
                {
                    new DailyReport{ ReportDate = DateTime.Today, User = new User{BodyTypeId = 1, Name = "User1", Surname = "TestUser1"}},
                    new DailyReport{ ReportDate = DateTime.Today, User = new User{BodyTypeId = 1, Name = "User2", Surname = "TestUser2"}},
                    new DailyReport{ ReportDate = DateTime.Today, User = new User{BodyTypeId = 2, Name = "User3", Surname = "TestUser3"}}
                });

            //Do test
            var result = await adminReportService.GetUsersDailyStatisticAsync(DateTime.Today);

            //Assert
            calculationsServiceMock.Verify(x => x.CalculateGroupsAverages(It.IsAny<IEnumerable<GroupStatistic>>()), Times.Once);

            Assert.Equal(2, result.Count());
            Assert.Equal(2, result.First().Values.Count());
            Assert.Single(result.Last().Values);
            Assert.Equal(TypeOfBody.Ectomorph, result.First().Key);
            Assert.Equal(TypeOfBody.Endomorph, result.Last().Key);
        }
    }
}
