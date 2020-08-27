using DietAssistant.DAL.Models;

namespace DietAssistant.Services.Interfaces
{
    public interface IDietService
    {
        void ValidateDailyReport(DailyReport report);
    }
}
