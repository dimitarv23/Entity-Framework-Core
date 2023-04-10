namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Where(c => c.Footballers.Any())
                .ToArray()
                .Select(c => new ExportCoachDto()
                {
                    FootballersCount = c.Footballers.Count,
                    Name = c.Name,
                    Footballers = c.Footballers.ToArray()
                        .OrderBy(f => f.Name)
                        .Select(f => new ExportFootballerDto()
                        {
                            Name = f.Name,
                            Position = f.PositionType.ToString()
                        }).ToList()
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.Name)
                .ToList();

            var xmlHelper = new XmlHelper();
            return xmlHelper.Serialize(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers
                    .Any(f => f.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                        .Where(tf => tf.Footballer.ContractStartDate >= date)
                        .OrderByDescending(f => f.Footballer.ContractEndDate)
                        .ThenBy(f => f.Footballer.Name)
                        .Select(tf => new
                        {
                            FootballerName = tf.Footballer.Name,
                            ContractStartDate = tf.Footballer.ContractStartDate
                                .ToString("d", CultureInfo.InvariantCulture),
                            ContractEndDate = tf.Footballer.ContractEndDate
                                .ToString("d", CultureInfo.InvariantCulture),
                            BestSkillType = tf.Footballer.BestSkillType.ToString(),
                            PositionType = tf.Footballer.PositionType.ToString()
                        })
                        .ToList()
                })
                .OrderByDescending(t => t.Footballers.Count)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToList();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
