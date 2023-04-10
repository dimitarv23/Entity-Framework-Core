using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models
{
    [Table("Teams")]
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [RegularExpression("^[a-zA-Z0-9 .-]{3,}$")]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; }

        [Required]
        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }
            = new List<TeamFootballer>();
    }
}
