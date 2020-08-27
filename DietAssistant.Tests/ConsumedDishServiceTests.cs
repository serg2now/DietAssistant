using AutoMapper;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Interfaces;
using DietAssistant.Services;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DietAssistant.Tests
{
    public class ConsumedDishServiceTests
    {
        private IConsumedDishService _dishService;
        private Mock<IRepository<ConsumedDish>> _dishRepositoryMock;
        private Mock<IReportService> _reportServiceMock;
        private Mock<IMapper> _mapperMock;

        public ConsumedDishServiceTests()
        {
            _dishRepositoryMock = new Mock<IRepository<ConsumedDish>>();
            _reportServiceMock = new Mock<IReportService>();

            _mapperMock = new Mock<IMapper>();
            _dishService = new ConsumedDishService(_dishRepositoryMock.Object, _reportServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddLogItemAsync()
        {
            //PrepareTest
            var logItem = new ConsumeLogItem() { CustomerId = 1, DateOfConsume = DateTime.Today};
            var consumedDish = new ConsumedDish();

            _dishRepositoryMock.Setup(x => x.AddItemAsync(consumedDish)).ReturnsAsync(1).Verifiable();
            _mapperMock.Setup(m => m.Map<ConsumedDish>(logItem)).Returns(consumedDish);
            _dishService.AutoUpdateDailyReport = true;

            //Do test
            var result = await _dishService.AddLogItemAsync(logItem);

            //Assert
            _reportServiceMock.Verify(s => s.UpsertDailyReportAsync(1, DateTime.Today), Times.Once);
            _dishRepositoryMock.Verify(r => r.AddItemAsync(consumedDish), Times.Once);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetLogItemsAsync()
        {
            //PrepareTest
            _dishRepositoryMock.Setup(x => x.GetItemsAsync(
                It.IsAny<Expression<Func<ConsumedDish, bool>>>(),
                It.IsAny<Func<IQueryable<ConsumedDish>, IOrderedQueryable<ConsumedDish>>>(),
                ""))
                .ReturnsAsync(new List<ConsumedDish>
                {
                    new ConsumedDish { Name = "Dish1"},
                    new ConsumedDish { Name = "Dish2" }
                });

            _mapperMock.Setup(m => m.Map<IEnumerable<ConsumeLogItem>>(It.IsAny<List<ConsumedDish>>()))
                .Returns<List<ConsumedDish>>(dishes => new List<ConsumeLogItem> 
                {
                    new ConsumeLogItem { Food = new Food { Name = dishes[0].Name } },
                    new ConsumeLogItem { Food = new Food { Name = dishes[1].Name } }
                });

            //Do test
            var result = await _dishService.GetLogItemsAsync(1, DateTime.Today);

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Dish1", result.First().Food.Name);
        }

        [Fact]
        public async Task AddLogItemAsync_IfFails_ReturnsZero()
        {
            //PrepareTest
            var logItem = new ConsumeLogItem();
            var consumedDish = new ConsumedDish();

            _dishRepositoryMock.Setup(x => x.AddItemAsync(consumedDish)).ReturnsAsync(-1).Verifiable();
            _mapperMock.Setup(m => m.Map<ConsumedDish>(logItem)).Returns(consumedDish);

            //Do test
            var result = await _dishService.AddLogItemAsync(logItem);

            //Assert
            _dishRepositoryMock.Verify(r => r.AddItemAsync(consumedDish), Times.Once);
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task UpdateLogItemAsync_WhenItemDoesNotExist_ThrowsError()
        {
            //Prepare test
            ConsumedDish dish = null;
            var logItem = new ConsumeLogItem { Id = 1};

            _dishRepositoryMock.Setup(a => a.GetItemAsync(1, "")).ReturnsAsync(dish);

            //Do Test and assert and Test
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _dishService.UpdateLogItem(logItem));
            Assert.Equal("Consumed dish with id 1 does not exist in database!", exception.Message);
        }

        [Fact]
        public async Task UpdateLogItemAsync()
        {
            //Prepare test
            ConsumedDish dish = new ConsumedDish();
            var logItem = new ConsumeLogItem { Id = 1, CustomerId = 1, DateOfConsume = DateTime.Today };

            _dishRepositoryMock.Setup(a => a.GetItemAsync(1, "")).ReturnsAsync(dish);
            _dishRepositoryMock.Setup(a => a.UpdateItemAsync(dish)).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<ConsumedDish>(logItem)).Returns(dish);
            _dishService.AutoUpdateDailyReport = true;

            //Do test
            var result = await _dishService.UpdateLogItem(logItem);

            //Assert
            _reportServiceMock.Verify(s => s.UpsertDailyReportAsync(1, DateTime.Today), Times.Once);
            Assert.Equal(1, result);
        }
    }
}
