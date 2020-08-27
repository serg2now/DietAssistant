using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DietAssistant.DAL.DataContext;
using DietAssistant.DAL.Models.Interfaces;
using DietAssistant.DAL.Repositories.Interfaces;

namespace DietAssistant.DAL.Repositories.Base
{
    public abstract class Repository<TModel> : ReadOnlyRepository<TModel>, IRepository<TModel>
        where TModel: class, IModel, new()
    {
        public Repository(DietAssistantContext context) : base(context) 
        {
        }

        public virtual async Task<int> AddItemAsync(TModel model)
        {
            _table.Add(model);

            var result = await SaveChangesAsync();

            return (result > 0) ? model.Id : result;
        }

        public abstract Task<int> UpdateItemAsync(TModel model);

        protected async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return -1;
            }
            catch (Exception)
            {
                return -2;
            }
        }
    }
}
