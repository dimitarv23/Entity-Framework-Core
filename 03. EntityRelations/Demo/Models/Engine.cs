using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class Engine
    {
        [Key]
        public int EngineId { get; set; }

        public string? Model { get; set; }

        public int Volume { get; set; }

        public virtual ICollection<CarEngine> CarsEngines { get; set; }
    }
}
