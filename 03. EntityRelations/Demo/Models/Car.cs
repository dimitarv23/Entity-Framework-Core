namespace Demo.Models
{
    public class Car
    {
        public int CarId { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int? Mileage { get; set; }

        public string? RegistrationPlate { get; set; }

        public virtual ICollection<CarEngine> CarsEngines { get; set; }
    }
}
