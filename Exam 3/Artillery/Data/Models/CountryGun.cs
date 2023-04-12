using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    [Table("CountriesGuns")]
    public class CountryGun
    {
        [Required]
        public int CountryId { get; set; }

        [Required]
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;

        [Required]
        public int GunId { get; set; }

        [Required]
        [ForeignKey(nameof(GunId))]
        public Gun Gun { get; set; } = null!;
    }
}
