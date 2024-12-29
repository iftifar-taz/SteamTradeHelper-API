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

        void Put(T obj);

        void PutAll(IEnumerable<T> objs);

        void Delete(T obj);

        void DeleteAll(IEnumerable<T> objs);
    }
}
