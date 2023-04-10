namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var xmlHelper = new XmlHelper();
            var sb = new StringBuilder();

            var coachDtos = xmlHelper
                .Deserialize<List<ImportCoachDto>>(xmlString, "Coaches");
            var coaches = new List<Coach>();

            foreach (var coachDto in coachDtos)
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var footballers = new List<Footballer>();

                foreach (var footballerDto in coachDto.Footballers.Distinct())
                {
                    if (!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (DateTime.ParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >
                        DateTime.ParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    footballers.Add(new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = DateTime.ParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ContractEndDate = DateTime.ParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                        PositionType = (PositionType)footballerDto.PositionType
                    });
                }

                var coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                    Footballers = footballers
                };

                coaches.Add(coach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var teamDtos = JsonConvert
                .DeserializeObject<List<ImportTeamDto>>(jsonString);

            var sb = new StringBuilder();
            var teams = new List<Team>();

            foreach (var teamDto in teamDtos)
            {
                if (!IsValid(teamDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (teamDto.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var teamFootballerIds = teamDto.FootballerIds;
                var teamFootballers = new List<TeamFootballer>();

                foreach (var footballerId in teamFootballerIds.Distinct())
                {
                    if (!context.Footballers.Any(f => f.Id == footballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    teamFootballers.Add(new TeamFootballer()
                    {
                        FootballerId = footballerId
                    });
                }

                var team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies,
                    TeamsFootballers = teamFootballers
                };
                teams.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            context.Teams.AddRange(teams);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
