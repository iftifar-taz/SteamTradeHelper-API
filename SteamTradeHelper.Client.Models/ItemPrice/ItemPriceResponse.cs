using System.Diagnostics.CodeAnalysis;

namespace SteamTradeHelper.Client.Models.ItemPrice
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Legacy code")]
    public class ItemPriceResponse
    {
        public bool? success { get; set; }

        public string? sell_order_table { get; set; }

        public string? sell_order_summary { get; set; }

        public string? buy_order_table { get; set; }

        public string? buy_order_summary { get; set; }

        public string? highest_buy_order { get; set; }

        public string? lowest_sell_order { get; set; }

        public IReadOnlyCollection<object>? buy_order_graph { get; set; }

        public IReadOnlyCollection<object>? sell_order_graph { get; set; }

        public string? graph_max_y { get; set; }

        public string? graph_min_x { get; set; }

        public string? graph_max_x { get; set; }

        public string? price_prefix { get; set; }

        public string? price_suffix { get; set; }
    }
}
