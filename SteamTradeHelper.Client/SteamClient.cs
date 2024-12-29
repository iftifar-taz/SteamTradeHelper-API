using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using SteamTradeHelper.Client.Contracts;
using SteamTradeHelper.Client.Models.Games;
using SteamTradeHelper.Client.Models.Inventories;
using SteamTradeHelper.Client.Models.ItemPrice;
using SteamTradeHelper.Client.Models.Steam;

namespace SteamTradeHelper.Client
{
    public class SteamClient(HttpClient _httpClient) : ISteamClient
    {
        private readonly HttpClient httpClient = _httpClient;
        private readonly Lazy<JsonSerializerSettings> settings = new(() =>
            {
                var settings = new JsonSerializerSettings();
                return settings;
            });

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public async Task<SwaggerResponse<GamesResponse>> GetSteamGamesAsync(string steamId, string key)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append("http://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={key}&steamid={steamId}&include_appinfo=true");
            urlBuilder_.Replace("{key}", Uri.EscapeDataString(ConvertToString(key, CultureInfo.InvariantCulture)));
            urlBuilder_.Replace("{steamId}", Uri.EscapeDataString(ConvertToString(steamId, CultureInfo.InvariantCulture)));

            using var request_ = new HttpRequestMessage();
            request_.Method = new HttpMethod("GET");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
            return await GetResponse<GamesResponse>(request_);
        }

        public async Task<string> GetSteamGameCardsPageAsync(int appId)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append("https://steamcommunity.com/profiles/76561198135500749/gamecards/{appId}");
            urlBuilder_.Replace("{appId}", Uri.EscapeDataString(ConvertToString(appId, CultureInfo.InvariantCulture)));

            using var request_ = new HttpRequestMessage();
            request_.Method = new HttpMethod("GET");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

            var response_ = await httpClient.GetByteArrayAsync(url_);
            var source_ = Encoding.GetEncoding("utf-8").GetString(response_, 0, response_.Length - 1);
            return WebUtility.HtmlDecode(source_);
        }

        public async Task<string> GetSteamCardPageAsync(string steamAppId, int appId, string cardName)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append("https://steamcommunity.com/market/listings/{steamAppId}/{appId}-{cardName}");
            urlBuilder_.Replace("{steamAppId}", Uri.EscapeDataString(ConvertToString(steamAppId, CultureInfo.InvariantCulture)));
            urlBuilder_.Replace("{appId}", Uri.EscapeDataString(ConvertToString(appId, CultureInfo.InvariantCulture)));
            urlBuilder_.Replace("{cardName}", Uri.EscapeDataString(ConvertToString(cardName, CultureInfo.InvariantCulture)));

            using var request_ = new HttpRequestMessage();
            request_.Method = new HttpMethod("GET");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

            var response_ = await httpClient.GetByteArrayAsync(url_);
            var source_ = Encoding.GetEncoding("utf-8").GetString(response_, 0, response_.Length - 1);
            return WebUtility.HtmlDecode(source_);
        }

        public async Task<SwaggerResponse<ItemPriceResponse>> GetItemPriceInformation(int itemId)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append("https://steamcommunity.com/market/itemordershistogram?country=AR&language=english&currency=34&item_nameid={itemId}");
            urlBuilder_.Replace("{itemId}", Uri.EscapeDataString(ConvertToString(itemId, CultureInfo.InvariantCulture)));

            using var request_ = new HttpRequestMessage();
            request_.Method = new HttpMethod("GET");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
            return await GetResponse<ItemPriceResponse>(request_);
        }

        public async Task<string> GetBotsPageAsync()
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append("https://asf.justarchi.net/STM");

            using var request_ = new HttpRequestMessage();
            request_.Method = new HttpMethod("GET");
            request_.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:75.0) Gecko/20100101 Firefox/75.0");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

            var response_ = await httpClient.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
            var content_ = await response_.Content.ReadAsByteArrayAsync();
            var source_ = Encoding.GetEncoding("utf-8").GetString(content_, 0, content_.Length - 1);
            return WebUtility.HtmlDecode(source_);
        }

        public async Task<SwaggerResponse<InventoryResponse>> GetSteamInventoryCountAsync(string steamId)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append("https://steamcommunity.com/inventory/{steamId}/753/6?count=1");
            urlBuilder_.Replace("{steamId}", Uri.EscapeDataString(ConvertToString(steamId, CultureInfo.InvariantCulture)));

            using var request_ = new HttpRequestMessage();
            request_.Method = new HttpMethod("GET");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
            return await GetResponse<InventoryResponse>(request_);
        }

        private async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(string.Empty, default);
            }

            try
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var streamReader = new StreamReader(responseStream);
                using var jsonTextReader = new JsonTextReader(streamReader);
                var serializer = JsonSerializer.Create(settings.Value);
                var typedBody = serializer.Deserialize<T>(jsonTextReader);
                return new ObjectResponseResult<T>(string.Empty, typedBody ?? default);
            }
            catch (JsonException exception)
            {
                var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                throw new SwaggerException(message, (int)response.StatusCode, string.Empty, headers, exception);
            }
        }

        private async Task<SwaggerResponse<T>> GetResponse<T>(HttpRequestMessage request_)
        {
            using var response_ = await httpClient.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false);
            var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null)
            {
                foreach (var item_ in response_.Content.Headers)
                {
                    headers_[item_.Key] = item_.Value;
                }
            }

            var status_ = ((int)response_.StatusCode).ToString();
            if (status_ == "200")
            {
                var objectResponse_ = await ReadObjectResponseAsync<T>(response_, headers_).ConfigureAwait(false);
                return new SwaggerResponse<T>((int)response_.StatusCode, headers_, objectResponse_.Object);
            }
            else if (status_ != "200" && status_ != "204")
            {
                var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new SwaggerException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_ ?? string.Empty, headers_, null);
            }

            return new SwaggerResponse<T>((int)response_.StatusCode, headers_, default);
        }

        private static string ConvertToString(object value, CultureInfo cultureInfo)
        {
            if (value is Enum)
            {
                var name = Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        if (CustomAttributeExtensions.GetCustomAttribute(field, typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
                        {
                            return attribute.Value ?? name;
                        }
                    }
                }
            }
            else if (value is bool)
            {
                return Convert.ToString(value, cultureInfo)?.ToLowerInvariant() ?? string.Empty;
            }
            else if (value is byte[] v)
            {
                return Convert.ToBase64String(v);
            }
            else if (value != null && value.GetType().IsArray)
            {
                var array = Enumerable.OfType<object>((Array)value);
                return string.Join(",", Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            return Convert.ToString(value, cultureInfo) ?? string.Empty;
        }
    }
}
