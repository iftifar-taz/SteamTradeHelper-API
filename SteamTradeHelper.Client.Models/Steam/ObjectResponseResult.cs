namespace SteamTradeHelper.Client.Models.Steam
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectResponseResult{T}"/> struct.
    /// </summary>
    /// <param name="responseObject">responseObject.</param>
    /// <param name="responseText">responseText.</param>
    public readonly struct ObjectResponseResult<T>(string responseText, T? responseObject)
    {

        /// <summary>
        /// Gets Object.
        /// </summary>
        public T? Object { get; } = responseObject;

        /// <summary>
        /// Gets Text.
        /// </summary>
        public string Text { get; } = responseText;
    }
}
