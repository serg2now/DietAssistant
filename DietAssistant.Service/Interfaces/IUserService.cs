using System.Collections.Generic;
using System.Threading.Tasks;
using DietAssistant.Services.DTOs;
using DietAssistant.Services.Enums;

namespace DietAssistant.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> AddNewUserAsync(SystemUser person, UserRole role);

        Task<IEnumerable<Customer>> GetCustomersAsync();

        Task<Customer> GetCustomerByIdAsync(int id);

        Task<int> UpdateCustomerAsync(Customer customer);

        Task<int> DeleteCustomerAsync(int id);
    }
}
