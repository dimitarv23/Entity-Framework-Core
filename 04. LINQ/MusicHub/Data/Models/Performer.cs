﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    [Table("Performers")]
    public class Performer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public decimal NetWorth { get; set; }

        public virtual ICollection<SongPerformer> PerformerSongs { get; set; }
    }
}
