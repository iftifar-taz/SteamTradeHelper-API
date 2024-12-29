namespace SteamTradeHelper.Client.Models.Steam
{
    public partial class SwaggerException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception? innerException)
        : Exception(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response[..(response.Length >= 512 ? 512 : response.Length)], innerException)
    {
        public int StatusCode { get; private set; } = statusCode;

        public string Response { get; private set; } = response;

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; } = headers;

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }
}
