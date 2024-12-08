using Microsoft.EntityFrameworkCore;
using SteamTradeHelper.Context;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;

namespace SteamTradeHelper.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : Base
    {
        private readonly DataContext context;
        private readonly DbSet<T> table;

        public BaseRepository(DataContext context)
        {
            this.context = context;
            this.table = context.Set<T>();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return this.table;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await this.table.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllQuery(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await this.table.FindAsync(id);
        }

        public virtual async Task<T> GetByIdQuery(int id, IQueryable<T> query)
        {
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<T> GetQuery(IQueryable<T> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task Save(T obj)
        {
            await this.table.AddAsync(obj);
            await this.context.SaveChangesAsync();
        }

        public virtual async Task SaveAll(IEnumerable<T> objs)
        {
            await this.table.AddRangeAsync(objs);
            await this.context.SaveChangesAsync();
        }

        public virtual async Task Put(T obj)
        {
            await this.context.SaveChangesAsync();
        }

        public virtual async Task PutAll(IEnumerable<T> objs)
        {
            await this.context.SaveChangesAsync();
        }

        public virtual async Task Delete(T obj)
        {
            this.table.Remove(obj);
            await this.context.SaveChangesAsync();
        }

        public virtual async Task DeleteAll(IEnumerable<T> objs)
        {
            this.table.RemoveRange(objs);
            await this.context.SaveChangesAsync();
        }
    }
}
