﻿using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        [Key]
        public int PositionId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PosititonNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
