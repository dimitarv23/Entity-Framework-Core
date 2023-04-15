using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models
{
    [Table("BoardgamesSellers")]
    public class BoardgameSeller
    {
        [Required]
        public int BoardgameId { get; set; }

        [Required]
        [ForeignKey(nameof(BoardgameId))]
        public Boardgame Boardgame { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        [ForeignKey(nameof(SellerId))]
        public Seller Seller { get; set; }
    }
}
