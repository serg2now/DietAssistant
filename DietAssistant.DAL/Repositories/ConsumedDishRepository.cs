using System.Threading.Tasks;
using DietAssistant.DAL.DataContext;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Base;

namespace DietAssistant.DAL.Repositories
{
    public class ConsumedDishRepository : Repository<ConsumedDish>
    {
        public ConsumedDishRepository(DietAssistantContext context) : base(context) { }

        public override async Task<int> UpdateItemAsync(ConsumedDish consumedDish)
        {
            var dbItem = await _table.FindAsync(consumedDish.Id);

            dbItem.Name = consumedDish.Name;
            dbItem.Description = consumedDish.Description;
            dbItem.ConsumeTimeTypeId = consumedDish.ConsumeTimeTypeId;
            dbItem.ProteinsAmount = consumedDish.ProteinsAmount;
            dbItem.FatsAmount = consumedDish.FatsAmount;
            dbItem.CarbohydratesAmount = consumedDish.CarbohydratesAmount;

            return await SaveChangesAsync();
        }
    }
}
