namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            IMapper mapper = CreateMapper();

            var xmlHelper = new XmlHelper();
            var despatcherDtos = xmlHelper
                .Deserialize<List<ImportDespatcherDto>>(xmlString, "Despatchers");

            var despatchers = new List<Despatcher>();
            StringBuilder sb = new StringBuilder();

            foreach (var despatcherDto in despatcherDtos)
            {
                if (!IsValid(despatcherDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var despatcher = mapper.Map<Despatcher>(despatcherDto);
                despatcher.Trucks = new List<Truck>();

                foreach (var truckDto in despatcherDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var truck = mapper.Map<Truck>(truckDto);
                    despatcher.Trucks.Add(truck);
                }

                despatchers.Add(despatcher);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher,
                    despatcher.Name, despatcher.Trucks.Count));
            }

            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportClient(TrucksContext context, string jsonString)
        {
            IMapper mapper = CreateMapper();

            var clientDtos =
                JsonConvert.DeserializeObject<List<ImportClientDto>>(jsonString);

            StringBuilder sb = new StringBuilder();
            var clients = new List<Client>();

            foreach (var clientDto in clientDtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var client = mapper.Map<Client>(clientDto);

                if (client.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var clientTruckIds = clientDto.TruckIds;
                foreach (var truckId in clientTruckIds.Distinct())
                {
                    if (!context.Trucks.Any(t => t.Id == truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    client.ClientsTrucks.Add(new ClientTruck
                    {
                        Client = client,
                        TruckId = truckId
                    });
                }

                clients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClient,
                    client.Name, client.ClientsTrucks.Count));
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TrucksProfile>();
            }));
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}