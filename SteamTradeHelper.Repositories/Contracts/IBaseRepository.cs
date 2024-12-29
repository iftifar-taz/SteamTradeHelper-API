namespace SteamTradeHelper.Repositories.Contracts
{
    public interface IBaseRepository<T>
        where T : class
    {
        IQueryable<T> GetQueryable();

        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> GetAllQuery(IQueryable<T> query);

        Task<T?> GetById(int id);

        Task<T?> GetByIdQuery(int id, IQueryable<T> query);

        Task<T?> GetQuery(IQueryable<T> query);

        Task Save(T obj);

        Task SaveAll(IEnumerable<T> objs);

        Task Put(T obj);

        Task PutAll(IEnumerable<T> objs);

        Task Delete(T obj);

        Task DeleteAll(IEnumerable<T> objs);
    }
}
