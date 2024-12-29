using SteamTradeHelper.Context;
using SteamTradeHelper.Context.Models;
using SteamTradeHelper.Repositories.Contracts;

namespace SteamTradeHelper.Repositories
{
    public class UnitOfWork(DataContext context) : IUnitOfWork
    {
        private readonly DataContext _context = context;
        private bool _disposed;

        public IBaseRepository<Bot> BotRepository { get; } = new BaseRepository<Bot>(context);
        public IBaseRepository<Card> CardRepository { get; } = new BaseRepository<Card>(context);
        public IBaseRepository<Game> GameRepository { get; } = new BaseRepository<Game>(context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
