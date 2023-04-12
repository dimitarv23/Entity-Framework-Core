namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var xmlHelper = new XmlHelper();

            var countryDtos = xmlHelper
                .Deserialize<List<ImportCountryDto>>(xmlString, "Countries");
            var countries = new List<Country>();

            var sb = new StringBuilder();

            foreach (var countryDto in countryDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                countries.Add(new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize,
                });

                sb.AppendLine(string.Format(SuccessfulImportCountry,
                    countryDto.CountryName, countryDto.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var xmlHelper = new XmlHelper();

            var manufacturerDtos = xmlHelper
                .Deserialize<List<ImportManufacturerDto>>(xmlString, "Manufacturers");
            var manufacturers = new List<Manufacturer>();

            var sb = new StringBuilder();

            foreach (var manufacturerDto in manufacturerDtos)
            {
                if (!IsValid(manufacturerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (manufacturers.Any(m =>
                    m.ManufacturerName == manufacturerDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                manufacturers.Add(new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded,
                });

                string[] townCountry = manufacturerDto.Founded
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                    .Reverse().Take(2).Reverse().ToArray();
                sb.AppendLine(string.Format(SuccessfulImportManufacturer,
                    manufacturerDto.ManufacturerName, string.Join(", ", townCountry)));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var xmlHelper = new XmlHelper();

            var shellDtos = xmlHelper
                .Deserialize<List<ImportShellDto>>(xmlString, "Shells");
            var shells = new List<Shell>();

            var sb = new StringBuilder();

            foreach (var shellDto in shellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                shells.Add(new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber,
                });

                sb.AppendLine(string.Format(SuccessfulImportShell,
                    shellDto.Caliber, shellDto.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var gunDtos = JsonConvert
                .DeserializeObject<List<ImportGunDto>>(jsonString);
            var guns = new List<Gun>();

            var sb = new StringBuilder();
            var validGunTypes = new string[]
            { "Howitzer", "Mortar", "FieldGun", "AntiAircraftGun", "MountainGun", "AntiTankGun", };

            foreach (var gunDto in gunDtos)
            {
                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!validGunTypes.Contains(gunDto.GunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var gunCountries = new List<CountryGun>();
                foreach (var countryDto in gunDto.Countries)
                {
                    gunCountries.Add(new CountryGun()
                    {
                        CountryId = countryDto.CountryId
                    });
                }

                guns.Add(new Gun
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = Enum.Parse<GunType>(gunDto.GunType),
                    ShellId = gunDto.ShellId,
                    CountriesGuns = gunCountries
                });

                sb.AppendLine(string.Format(SuccessfulImportGun,
                    gunDto.GunType.ToString(), gunDto.GunWeight, gunDto.BarrelLength));
            }

            context.Guns.AddRange(guns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}