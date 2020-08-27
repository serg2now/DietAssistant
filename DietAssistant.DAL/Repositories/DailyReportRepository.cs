using System.Threading.Tasks;
using DietAssistant.DAL.DataContext;
using DietAssistant.DAL.Models;
using DietAssistant.DAL.Repositories.Base;

namespace DietAssistant.DAL.Repositories
{
    public class DailyReportRepository : Repository<DailyReport>
    {
        public DailyReportRepository(DietAssistantContext context) : base(context)
        { }

        public override async Task<int> UpdateItemAsync(DailyReport model)
        {
            var dbItem = await _table.FindAsync(model.Id);

            dbItem.ProteinsAmount = model.ProteinsAmount;
            dbItem.CarbohydratesAmount = model.CarbohydratesAmount;
            dbItem.FatsAmount = model.FatsAmount;
            dbItem.HasWarnings = model.HasWarnings;
            dbItem.Warnings = model.Warnings;

            return await SaveChangesAsync();
        }
    }
}
