using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace Artillery.Data.Models
{
    [Table("Manufacturers")]
    [Index(nameof(ManufacturerName), IsUnique = true)]
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(40)]
        public string ManufacturerName { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Founded { get; set; }

        public virtual ICollection<Gun> Guns { get; set; }
            = new List<Gun>();
    }
}
