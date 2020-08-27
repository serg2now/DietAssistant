using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DietAssistant.DAL.DataContext;
using DietAssistant.DAL.Models.Interfaces;
using DietAssistant.DAL.Repositories.Interfaces;

namespace DietAssistant.DAL.Repositories.Base
{
    public class ReadOnlyRepository<TModel> : IReadOnlyRepository<TModel>
        where TModel: class, IModel, new()
    {
        protected readonly DietAssistantContext _dbContext;
        protected readonly DbSet<TModel> _table;

        public ReadOnlyRepository(DietAssistantContext context)
        {
            _dbContext = context;
            _table = _dbContext.Set<TModel>();
        }

        public virtual async Task<TModel> GetItemAsync(int id, string includes = "")
        {
            try
            {
                var query = _table.AsNoTracking();
                query = IncludeFields(query, includes);

                return await query.FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<IEnumerable<TModel>> GetItemsAsync(
            Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            string includes = "")
        {
            try
            {
                var query = _table.AsNoTracking();

                query = IncludeFields(query, includes);

                query = (filter != null) ? query.Where(filter) : query;

                query = (orderBy != null) ? orderBy(query) : query;

                return await query.ToArrayAsync();
            }
            catch (Exception)
            {
                return new List<TModel>();
            }
        }

        protected IQueryable<TModel> IncludeFields(IQueryable<TModel> query, string includes)
        {
            if (!string.IsNullOrEmpty(includes))
            {
                var fields = includes.Split(',');

                foreach (var field in fields)
                {
                    query = query.Include(field);
                }
            }

            return query;
        }
    }
}
