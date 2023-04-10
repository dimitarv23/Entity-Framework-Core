namespace Trucks
{
    using AutoMapper;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ImportDto;

    public class TrucksProfile : Profile
    {
        public TrucksProfile()
        {
            this.CreateMap<ImportDespatcherDto, Despatcher>();
            this.CreateMap<ImportTruckDto, Truck>();
            this.CreateMap<ImportClientDto, Client>();
        }
    }
}
