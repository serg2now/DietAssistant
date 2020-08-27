using DietAssistant.DAL.Models;
using System.Threading.Tasks;

namespace DietAssistant.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> DeleteItemAsync(User user);
    }
}
