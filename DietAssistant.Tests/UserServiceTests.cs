using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Interfaces;
using DietAssistant.Services;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;
using DietAssistant.Services.Interfaces;
using Moq;
using Xunit;

namespace DietAssistant.Tests
{
    public class UserServiceTests
    {
        private IUserService usersService;
        private Mock<IUserRepository> usersRepositoryMock;
        private Mock<IMapper> mapperMock;

        public UserServiceTests()
        {
            usersRepositoryMock = new Mock<IUserRepository>();

            mapperMock = new Mock<IMapper>();
            usersService = new UserService(usersRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task GetCustomersAsync()
        {
            //PrepareTest
            usersRepositoryMock.Setup(x => x.GetItemsAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                ""))
                .ReturnsAsync(new List<User>
                {
                    new User { Name = "Customer1"},
                    new User { Name = "Customer2" }
                });

            mapperMock.Setup(m => m.Map<IEnumerable<Customer>>(It.IsAny<List<User>>()))
                .Returns<List<User>>(users => new List<Customer>
                {
                    new Customer {  Name = users[0].Name  },
                    new Customer {  Name = users[1].Name  }
                });

            //Do test
            var result = await usersService.GetCustomersAsync();

            //Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Customer1", result.First().Name);
        }

        [Fact]
        public async Task GetCustomerByIdAsync()
        {
            //Prepare test
            usersRepositoryMock
                .Setup(x => x.GetItemAsync(1, ""))
                .ReturnsAsync(new User { Name = "User1" });

            mapperMock
                .Setup(m => m.Map<Customer>(It.IsAny<User>()))
                .Returns<User>(user => new Customer { Name = user.Name });

            //Do test
            var result = await usersService.GetCustomerByIdAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("User1", result.Name);
        }

        [Fact]
        public async Task DeleteCustomerAsync_WhenUserDoesNotExists_ThrowError()
        {
            //Prepare test
            User user = null;
            usersRepositoryMock.Setup(a => a.GetItemAsync(1, "")).ReturnsAsync(user);

            //Do Test and assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await  usersService.DeleteCustomerAsync(1));
            Assert.Equal("User With id 1 does not exist in database!", exception.Message);
        }

        [Fact]
        public async Task DeleteCustomerAsync()
        {
            //Prepare test
            var user = new User() { Id = 1 };

            usersRepositoryMock.Setup(a => a.GetItemAsync(1, "")).ReturnsAsync(user);
            usersRepositoryMock.Setup(a => a.DeleteItemAsync(user)).ReturnsAsync(1);

            //Do test
            var result = await usersService.DeleteCustomerAsync(1);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task UpdateCustomerAsync()
        {
            //Prepare test
            var user = new User() { Id = 1 };
            var customer = new Customer() { Id = 1 };

            usersRepositoryMock.Setup(a => a.GetItemAsync(1, "")).ReturnsAsync(user);
            usersRepositoryMock.Setup(a => a.UpdateItemAsync(user)).ReturnsAsync(1);
            mapperMock.Setup(m => m.Map<User>(customer)).Returns(user);

            //Do test
            var result = await usersService.UpdateCustomerAsync(customer);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddNewCustomerUserAsync()
        {
            //Prepare test
            var customer = new Customer();
            var user = new User();

            mapperMock.Setup(m => m.Map<User>(customer)).Returns(user).Verifiable();
            usersRepositoryMock.Setup(a => a.AddItemAsync(user)).ReturnsAsync(1);

            //Do test
            var result = await usersService.AddNewUserAsync(customer, UserRole.Customer);

            //Assert
            mapperMock.Verify(m => m.Map<User>(customer), Times.Once);

            Assert.Equal(1, result);
            Assert.Equal((int)UserRole.Customer, user.RoleId);
        }

        [Fact]
        public async Task AddNewCustomerAsync_IfFail_ReturnsErrorNumder()
        {
            //Prepare test
            var customer = new Customer ();
            var user = new User ();

            mapperMock.Setup(m => m.Map<User>(customer)).Returns(user);
            usersRepositoryMock.Setup(a => a.AddItemAsync(user)).ReturnsAsync(-1);

            //Do test
            var result = await usersService.AddNewUserAsync(customer, UserRole.Customer);

            //Assert
            Assert.Equal(-1, result);
        }
    }
}
