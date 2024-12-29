using System.ComponentModel.DataAnnotations;

namespace SteamTradeHelper.Context.Models
{
    public class Base
    {
        [Key]
        public int Id { get; set; }

        public required string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Base()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
