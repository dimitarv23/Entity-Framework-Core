using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        [Required]
        public int PlayId { get; set; }

        [Required]
        [ForeignKey(nameof(PlayId))]
        public virtual Play Play { get; set; }

        [Required]
        public int TheatreId { get; set; }

        [Required]
        [ForeignKey(nameof(TheatreId))]
        public virtual Theatre Theatre { get; set; }
    }
}
