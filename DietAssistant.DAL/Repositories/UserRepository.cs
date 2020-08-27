using System.Threading.Tasks;
using DietAssistant.DAL.DataContext;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Base;
using DietAssistant.DAL.Repositories.Interfaces;

namespace DietAssistant.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DietAssistantContext context) : base(context) 
        { }

        public override async Task<int> UpdateItemAsync(User user)
        {
            var dbUser = await _table.FindAsync(user.Id);

            dbUser.WeightInKilos = user.WeightInKilos;
            dbUser.HeightInMeters = user.HeightInMeters;

            return await SaveChangesAsync();
        }

        public virtual async Task<int> DeleteItemAsync(User user)
        {
            var dbUser = await _table.FindAsync(user.Id);

            _table.Remove(dbUser);

            return await SaveChangesAsync();
        } 
    }
}
