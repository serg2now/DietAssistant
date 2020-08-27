using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Interfaces;
using DietAssistant.Services;
using DietAssistant.Services.Interfaces;
using DietAssistant.Services.MappingConfigurations;

namespace DietAssistant.Tests
{
    public class ReportServiceTests
    {
        private Mock<IRepository<DailyReport>> reportsRepositoryMock;
        private Mock<IRepository<ConsumedDish>> dishRepositoryMock;
        private Mock<IUserRepository> usersRepositoryMock;
        private Mock<IParametersCalculationService> calculationsServiceMock;
        private Mock<IDietService> dietServiceMock;
        private IReportService reportService;

        private User user;
        private List<ConsumedDish> dishes;

        public ReportServiceTests()
        {
            reportsRepositoryMock = new Mock<IRepository<DailyReport>>();
            dishRepositoryMock = new Mock<IRepository<ConsumedDish>>();
            usersRepositoryMock = new Mock<IUserRepository>();
            calculationsServiceMock = new Mock<IParametersCalculationService>();
            dietServiceMock = new Mock<IDietService>();

            user = new User { Name = "Test", Surname = "User" };
            dishes = new List<ConsumedDish>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new DailyReportMappingProfile()));
            var mapper = new Mapper(configuration);

            SetupMockObjects();   

            reportService = new ReportService(
                reportsRepositoryMock.Object,
                usersRepositoryMock.Object,
                dishRepositoryMock.Object,
                calculationsServiceMock.Object,
                dietServiceMock.Object,
                mapper);
        }

        private void SetupMockObjects()
        {
            dietServiceMock.Setup(s => s.ValidateDailyReport(It.IsAny<DailyReport>()))
                .Callback<DailyReport>((report) =>
                {
                    report.HasWarnings = false;
                    report.Warnings = "";
                })
                .Verifiable();

            usersRepositoryMock.Setup(r => r.GetItemAsync(1, ""))
                .ReturnsAsync(user)
                .Verifiable();

            dishRepositoryMock.Setup(r => r.GetItemsAsync(It.IsAny<Expression<Func<ConsumedDish, bool>>>(), null, ""))
                .ReturnsAsync(dishes);

            calculationsServiceMock.Setup(s => s.CalculateDailyAmount(dishes, It.IsAny<DailyReport>()))
                .Callback<IEnumerable<ConsumedDish>, DailyReport>((dishes, report) =>
                {
                    report.FatsAmount = 100;
                })
                .Verifiable();
        }

        [Fact]
        public async Task GetAllReportsAsync()
        {
            //PrepareTest
            reportsRepositoryMock.Setup(x => x.GetItemsAsync(
                null,
                It.IsAny<Func<IQueryable<DailyReport>, IOrderedQueryable<DailyReport>>>(),
                "User"))
                .ReturnsAsync(new List<DailyReport>
                {
                    new DailyReport { UserId = 1, ReportDate = DateTime.Today.AddDays(-1), User = new User{ Name = "User1", Surname = "Test1" } },
                    new DailyReport { UserId = 2, ReportDate = DateTime.Today, User = new User{ Name = "User2", Surname = "Test2" } }
                });

            //Do test
            var result = await reportService.GetAllReportsAsync();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(DateTime.Today.AddDays(-1), result.First().ReportDate);
            Assert.Equal("User2", result.Last().CustomerName);
        }

        [Fact]
        public async Task GetReportsByDateAsync()
        {
            //PrepareTest
            reportsRepositoryMock.Setup(x => x.GetItemsAsync(
                It.IsAny<Expression<Func<DailyReport, bool>>>(),
                It.IsAny<Func<IQueryable<DailyReport>, IOrderedQueryable<DailyReport>>>(),
                "User"))
                .ReturnsAsync(new List<DailyReport>
                {
                    new DailyReport { UserId = 1, ReportDate = DateTime.Today, User = new User{ Name = "User1", Surname = "Test1" } },
                    new DailyReport { UserId = 2, ReportDate = DateTime.Today, User = new User{ Name = "User2", Surname = "Test2" } }
                });

            //Do test
            var result = await reportService.GetReportsByDateAsync(DateTime.Today);

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(DateTime.Today, result.First().ReportDate);
            Assert.Equal("User2", result.Last().CustomerName);
        }

        [Fact]
        public async Task GetDailyReportAsync()
        {
            //PrepareTest
            reportsRepositoryMock.Setup(x => x.GetItemsAsync(
                It.IsAny<Expression<Func<DailyReport, bool>>>(),
                It.IsAny<Func<IQueryable<DailyReport>, IOrderedQueryable<DailyReport>>>(),
                "User"))
                .ReturnsAsync(new List<DailyReport>
                {
                    new DailyReport
                    {
                        UserId = 1,
                        ReportDate = DateTime.Today,
                        User = new User{ Name = "User1", Surname = "Test1" },
                        HasWarnings = true,
                        Warnings = "Some Warning"
                    }
                });

            //Do test
            var result = await reportService.GetDailyReportAsync(1, DateTime.Today);

            //Assert
            Assert.Equal(DateTime.Today, result.ReportDate);
            Assert.Equal("User1", result.CustomerName);
            Assert.True(result.HasWarnings);
            Assert.Equal("Some Warning", result.Warnings);
        }

        [Fact]
        public async Task UpsertDailyReportAsync_IfNoDishes_ReturnsEmptyReport()
        {
            //Prepare test
            dishRepositoryMock.Setup(r => r.GetItemsAsync(x => It.IsAny<bool>(), null, null))
                .ReturnsAsync(new List<ConsumedDish>());

            //Do test
            var report = await reportService.UpsertDailyReportAsync(1, DateTime.Today);

            //Assert
            Assert.True(report.HasWarnings);
            Assert.Equal("There are no consume data for daily report!", report.Warnings);
        }

        [Fact]
        public async Task UpsertDailyReportAsync_IfRepotDoesNotExist_CreatesNewReport()
        {
            //Prepare test
            var dish = new ConsumedDish ();
            dishes.Add(dish);

            reportsRepositoryMock.Setup(r => r.GetItemsAsync(It.IsAny<Expression<Func<DailyReport, bool>>>(), null, "User"))
                .ReturnsAsync(new List<DailyReport>());

            reportsRepositoryMock.Setup(r => r.AddItemAsync(It.IsAny<DailyReport>()))
                .ReturnsAsync(1)
                .Verifiable();

            //Do test
            var result = await reportService.UpsertDailyReportAsync(1, DateTime.Today);

            //Assert
            calculationsServiceMock.Verify(x => x.CalculateDailyAmount(dishes, It.IsAny<DailyReport>()), Times.Once);
            dietServiceMock.Verify(x => x.ValidateDailyReport(It.IsAny<DailyReport>()), Times.Once);
            reportsRepositoryMock.Verify(x => x.AddItemAsync(It.IsAny<DailyReport>()), Times.Once);
            usersRepositoryMock.Verify(x => x.GetItemAsync(1, ""), Times.Once);

            Assert.False(result.HasWarnings);
            Assert.Equal(100, result.FatsAmount);
            Assert.Equal("", result.Warnings);
            Assert.Equal(DateTime.Today, result.ReportDate);
            Assert.Equal(user.Name, result.CustomerName);
            Assert.Equal(user.Surname, result.CustomerSurname);
        }

        [Fact]
        public async Task UpsertDailyReportAsync_IfReportExists_UpdateReport()
        {
            //Prepare test
            if (!dishes.Any())
            {
                var dish = new ConsumedDish();
                dishes.Add(dish);
            }

            reportsRepositoryMock.Setup(r => r.GetItemsAsync(It.IsAny<Expression<Func<DailyReport, bool>>>(), null, "User"))
                .ReturnsAsync(new List<DailyReport>() { new DailyReport { Id = 1, User = user, ReportDate = DateTime.Today } });

            reportsRepositoryMock.Setup(r => r.UpdateItemAsync(It.IsAny<DailyReport>()))
                .ReturnsAsync(1)
                .Verifiable();

            //Do test
            var result = await reportService.UpsertDailyReportAsync(1, DateTime.Today);

            //Assert
            reportsRepositoryMock.Verify(x => x.UpdateItemAsync(It.IsAny<DailyReport>()), Times.Once);
            usersRepositoryMock.Verify(x => x.GetItemAsync(1, ""), Times.Never);

            Assert.False(result.HasWarnings);
            Assert.Equal(100, result.FatsAmount);
            Assert.Equal("", result.Warnings);
            Assert.Equal(DateTime.Today, result.ReportDate);
            Assert.Equal(user.Name, result.CustomerName);
            Assert.Equal(user.Surname, result.CustomerSurname);
        }

        [Fact]
        public async Task UpsertDailyReportAsync_IfDbUpdateFailed_ReturnsEmptyReport()
        {
            //Prepare test
            if (!dishes.Any())
            {
                var dish = new ConsumedDish();
                dishes.Add(dish);
            }

            reportsRepositoryMock.Setup(r => r.GetItemsAsync(It.IsAny<Expression<Func<DailyReport, bool>>>(), null, "User"))
                .ReturnsAsync(new List<DailyReport>());

            reportsRepositoryMock.Setup(r => r.AddItemAsync(It.IsAny<DailyReport>()))
                .ReturnsAsync(-1)
                .Verifiable();

            //Do test
            var result = await reportService.UpsertDailyReportAsync(1, DateTime.Today);

            //Assert
            reportsRepositoryMock.Verify(x => x.AddItemAsync(It.IsAny<DailyReport>()), Times.Once);
            usersRepositoryMock.Verify(x => x.GetItemAsync(1, ""), Times.Never);

            Assert.Equal(DateTime.Today, result.ReportDate);
            Assert.Null(result.CustomerName);
            Assert.Null(result.CustomerSurname);
            Assert.Null(result.Warnings);
            Assert.False(result.HasWarnings);
            Assert.Equal(0, result.FatsAmount);
        }
    }
}
