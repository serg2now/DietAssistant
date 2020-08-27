using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Interfaces;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;
using DietAssistant.Services.Interfaces;

namespace DietAssistant.Services
{
    public class UserService : IUserService
    {
        protected readonly IUserRepository _userRepository;
        protected readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            var dbUsers = await _userRepository.GetItemsAsync(
                c => c.RoleId == (int)UserRole.Customer,
                q => q.OrderBy(u => u.Surname));

            return _mapper.Map<IEnumerable<Customer>>(dbUsers);
        }

        public virtual async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var user = await _userRepository.GetItemAsync(id);

            return _mapper.Map<Customer>(user);
        }

        public virtual async Task<int> AddNewUserAsync(SystemUser person, UserRole role)
        {
            var dbUser = _mapper.Map<User>(person);

            dbUser.RoleId = (int)role;

            var result = await _userRepository.AddItemAsync(dbUser);

            return result;
        }

        public virtual async Task<int> UpdateCustomerAsync(Customer customer)
        {
            await CheckIfUserExists(customer.Id);

            var dbUser = _mapper.Map<User>(customer);

            return await _userRepository.UpdateItemAsync(dbUser);
        }

        public virtual async Task<int> DeleteCustomerAsync(int id)
        {
            var dbUser = await CheckIfUserExists(id);

            return await _userRepository.DeleteItemAsync(dbUser);
        }

        protected async Task<User> CheckIfUserExists(int userId)
        {
            var dbUser = await _userRepository.GetItemAsync(userId);

            if (dbUser == null)
            {
                throw new ArgumentException($"User With id {userId} does not exist in database!");
            }

            return dbUser;
        }

    }
}
