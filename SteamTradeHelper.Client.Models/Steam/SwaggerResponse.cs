using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Steam
{
    public partial class SwaggerResponse
    {
        public SwaggerResponse(int statusCode, IReadOnlyDictionary<string, IEnumerable<string>> headers)
        {
            this.StatusCode = statusCode;
            this.Headers = headers;
        }

        public int StatusCode { get; private set; }

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }
    }

    public partial class SwaggerResponse<TResult> : SwaggerResponse
    {
        public SwaggerResponse(int statusCode, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result)
            : base(statusCode, headers)
        {
            this.Result = result;
        }

        public TResult Result { get; private set; }
    }
}
