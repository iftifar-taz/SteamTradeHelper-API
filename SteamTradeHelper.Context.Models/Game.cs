using System.ComponentModel.DataAnnotations;

namespace SteamTradeHelper.Context.Models
{
    public class Game : Base
    {
        [Required]
        public int AppId { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? IconHash { get; set; }

        public string? LogoHash { get; set; }

        public bool IsTradeable { get; set; }

        public virtual ICollection<Card>? Cards { get; set; }
    }
}
