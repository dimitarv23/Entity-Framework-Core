using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    [Table("SongsPerformers")]
    public class SongPerformer
    {
        public int SongId { get; set; }

        [Required]
        [ForeignKey(nameof(SongId))]
        public virtual Song Song { get; set; }

        public int PerformerId { get; set; }

        [Required]
        [ForeignKey(nameof(PerformerId))]
        public virtual Performer Performer { get; set; }
    }
}
