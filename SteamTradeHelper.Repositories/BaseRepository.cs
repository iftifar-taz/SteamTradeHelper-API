using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;

namespace SteamTradeHelper.Repositories
{
    public class BaseRepository<T>(DataContext context) : IBaseRepository<T>
        where T : Base
    {
        private readonly DataContext context = context;
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
            await context.SaveChangesAsync();
        }

        public virtual async Task SaveAll(IEnumerable<T> objs)
        {
            await entity.AddRangeAsync(objs);
            await context.SaveChangesAsync();
        }

        public virtual async Task Put(T obj)
        {
            await context.SaveChangesAsync();
        }

        public virtual async Task PutAll(IEnumerable<T> objs)
        {
            await context.SaveChangesAsync();
        }

        public virtual async Task Delete(T obj)
        {
            entity.Remove(obj);
            await context.SaveChangesAsync();
        }

        public virtual async Task DeleteAll(IEnumerable<T> objs)
        {
            entity.RemoveRange(objs);
            await context.SaveChangesAsync();
        }
    }
}
