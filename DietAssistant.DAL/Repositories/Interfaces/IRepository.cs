using DietAssistant.DAL.Models.Interfaces;
using System.Threading.Tasks;

namespace DietAssistant.DAL.Repositories.Interfaces
{
    public interface IRepository<TModel> : IReadOnlyRepository<TModel>
        where TModel: class, IModel, new()
    {
        Task<int> AddItemAsync(TModel model);

        Task<int> UpdateItemAsync(TModel model);
    }
}
