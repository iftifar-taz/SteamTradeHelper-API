namespace SteamTradeHelper.Dtos
{
    public class ListResponse<T>
        where T : class
    {
        public int Total { get; set; }

        public DateTime LastSynced { get; set; }

        public IReadOnlyCollection<T>? List { get; set; }
    }
}
