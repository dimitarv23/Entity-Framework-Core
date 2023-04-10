using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    [Table("Trucks")]
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [StringLength(8, MinimumLength = 8)]
        public string RegistrationNumber { get; set; }

        [Required]
        [StringLength(17, MinimumLength = 17)]
        public string VinNumber { get; set; }

        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [Required]
        public int DespatcherId { get; set; }

        [ForeignKey(nameof(DespatcherId))]
        public virtual Despatcher Despatcher { get; set; }

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
            = new List<ClientTruck>();
    }
}
