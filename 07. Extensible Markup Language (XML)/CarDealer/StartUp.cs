using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using ProductShop.Utilities;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext dbContext = new CarDealerContext();

            var inputXml = File.ReadAllText("../../../Datasets/customers.xml");

            Console.WriteLine(
                ImportCustomers(dbContext, inputXml));
        }

        //Exercise 9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();

            var xmlHelper = new XmlHelper();
            var supplierDtos = xmlHelper
                .Deserialize<List<ImportSupplierDto>>(inputXml, "Suppliers");

            var suppliers = new List<Supplier>();

            foreach (var supplierDto in supplierDtos)
            {
                var supplier = mapper.Map<Supplier>(supplierDto);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        //Exercise 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();

            var xmlHelper = new XmlHelper();
            var carDtos = xmlHelper
                .Deserialize<List<ImportPartDto>>(inputXml, "Parts");

            var parts = new List<Part>();
            List<int> validSupplierIds = context.Suppliers
                .Select(s => s.Id).ToList();

            foreach (var partDto in carDtos
                .Where(p => validSupplierIds.Contains(p.SupplierId)))
            {
                var part = mapper.Map<Part>(partDto);
                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        //Exercise 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();

            var xmlHelper = new XmlHelper();
            var carDtos = xmlHelper
                .Deserialize<List<ImportCarDto>>(inputXml, "Cars");

            var cars = new List<Car>();

            foreach (var carDto in carDtos
                .Where(c => !string.IsNullOrWhiteSpace(c.Make) &&
                            !string.IsNullOrWhiteSpace(c.Model)))
            {
                var car = mapper.Map<Car>(carDto);

                var parts = new List<Part>();
                foreach (var carPartDto in carDto.Parts
                    .DistinctBy(p => p.PartId))
                {
                    if (context.Parts.Any(p => p.Id == carPartDto.PartId))
                    {
                        var carPart = new PartCar()
                        {
                            PartId = carPartDto.PartId
                        };

                        car.PartsCars.Add(carPart);
                    }
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Exercise 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMap();

            var xmlHelper = new XmlHelper();
            var customerDtos = xmlHelper
                .Deserialize<List<ImportCustomerDto>>(inputXml, "Customers");

            var customers = new List<Customer>();

            foreach (var customerDto in customerDtos)
            {
                var customer = mapper.Map<Customer>(customerDto);
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        private static IMapper CreateMap()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
    }
}