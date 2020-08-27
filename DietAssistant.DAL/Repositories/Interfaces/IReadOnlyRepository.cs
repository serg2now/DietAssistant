using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DietAssistant.DAL.Models.Interfaces;

namespace DietAssistant.DAL.Repositories.Interfaces
{
    public interface IReadOnlyRepository<TModel> where TModel: IModel , new()
    {
        Task<IEnumerable<TModel>> GetItemsAsync(
            Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            string includes = "");

        Task<TModel> GetItemAsync(int id, string includes = "");
    }
}
