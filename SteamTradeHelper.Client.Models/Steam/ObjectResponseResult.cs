using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTradeHelper.Client.Models.Steam
{
    public struct ObjectResponseResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectResponseResult{T}"/> struct.
        /// </summary>
        /// <param name="responseObject">responseObject.</param>
        /// <param name="responseText">responseText.</param>
        public ObjectResponseResult(T responseObject, string responseText)
        {
            this.Object = responseObject;
            this.Text = responseText;
        }

        /// <summary>
        /// Gets Object.
        /// </summary>
        public T Object { get; }

        /// <summary>
        /// Gets Text.
        /// </summary>
        public string Text { get; }
    }
}
