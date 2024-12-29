using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;

namespace SteamTradeHelper.Repositories
{
    public class BaseRepository<T>(DataContext context) : IBaseRepository<T>
        where T : Base
    {
        private readonly DbSet<T> entity = context.Set<T>();

        public virtual IQueryable<T> GetQueryable()
        {
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await entity.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllQuery(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetById(int id)
        {
            return await entity.FindAsync(id) ?? null;
        }

        public virtual async Task<T?> GetByIdQuery(int id, IQueryable<T> query)
        {
            return await query.FirstOrDefaultAsync(x => x.Id == id) ?? null;
        }

        public virtual async Task<T?> GetQuery(IQueryable<T> query)
        {
            return await query.FirstOrDefaultAsync() ?? null;
        }

        public virtual async Task Save(T obj)
        {
            await entity.AddAsync(obj);
        }

        public virtual async Task SaveAll(IEnumerable<T> objs)
        {
            await entity.AddRangeAsync(objs);
        }

        public virtual void Put(T obj)
        {
            entity.Update(obj);
        }

        public virtual void PutAll(IEnumerable<T> objs)
        {
            entity.UpdateRange(objs);
        }

        public virtual void Delete(T obj)
        {
            entity.Remove(obj);
        }

        public virtual void DeleteAll(IEnumerable<T> objs)
        {
            entity.RemoveRange(objs);
        }
    }
}
