using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class CarEngine
    {
        public int CarId { get; set; }

        [ForeignKey(nameof(CarId))]
        public virtual Car Car { get; set; }

        public int EngineId { get; set; }

        [ForeignKey(nameof(EngineId))]
        public virtual Engine Engine { get; set; }
    }
}
