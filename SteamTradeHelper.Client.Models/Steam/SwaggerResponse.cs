namespace SteamTradeHelper.Client.Models.Steam
{
    public partial class SwaggerResponse(int statusCode, IReadOnlyDictionary<string, IEnumerable<string>> headers)
    {
        public int StatusCode { get; private set; } = statusCode;

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; } = headers;
    }

    public partial class SwaggerResponse<TResult>(int statusCode, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult? result) : SwaggerResponse(statusCode, headers)
    {
        public TResult? Result { get; private set; } = result;
    }
}
