using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Footballers.Data.Models.Enums;

namespace Footballers.Data.Models
{
    [Table("Footballers")]
    public class Footballer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public DateTime ContractStartDate { get; set; }

        [Required]
        public DateTime ContractEndDate { get; set; }

        [Required]
        public PositionType PositionType { get; set; }

        [Required]
        public BestSkillType BestSkillType { get; set; }

        [Required]
        public int CoachId { get; set; }

        [ForeignKey(nameof(CoachId))]
        public virtual Coach Coach { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }
            = new List<TeamFootballer>();
    }
}
