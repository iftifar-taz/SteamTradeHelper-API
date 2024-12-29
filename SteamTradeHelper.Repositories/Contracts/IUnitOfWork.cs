using SteamTradeHelper.Context.Models;

namespace SteamTradeHelper.Repositories.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Bot> BotRepository { get; }
        IBaseRepository<Card> CardRepository { get; }
        IBaseRepository<Game> GameRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
