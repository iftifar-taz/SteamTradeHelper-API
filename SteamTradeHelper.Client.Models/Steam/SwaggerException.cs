using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Steam
{
    public partial class SwaggerException : Exception
    {
        public SwaggerException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length), innerException)
        {
            this.StatusCode = statusCode;
            this.Response = response;
            this.Headers = headers;
        }

        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", this.Response, base.ToString());
        }
    }
}
