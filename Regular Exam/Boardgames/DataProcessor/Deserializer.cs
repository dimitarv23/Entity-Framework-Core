namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var xmlHelper = new XmlHelper();
            var creatorDtos = xmlHelper
                .Deserialize<List<ImportCreatorDto>>(xmlString, "Creators");

            var sb = new StringBuilder();
            var creators = new List<Creator>();

            foreach (var dto in creatorDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var boardgames = new List<Boardgame>();

                foreach (var boardgameDto in dto.Boardgames)
                {
                    if (!IsValid(boardgameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    boardgames.Add(new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (CategoryType)boardgameDto.CategoryType,
                        Mechanics = boardgameDto.Mechanics
                    });
                }

                creators.Add(new Creator()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Boardgames = boardgames
                });
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, 
                    dto.FirstName, dto.LastName, boardgames.Count));
            }

            context.Creators.AddRange(creators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sellerDtos = JsonConvert
                .DeserializeObject<List<ImportSellerDto>>(jsonString);
            var validBoardgameIds = context.Boardgames
                .Select(b => b.Id).ToList();

            var sb = new StringBuilder();
            var sellers = new List<Seller>();

            foreach (var dto in sellerDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var sellerBoardgames = new List<BoardgameSeller>();

                foreach (var id in dto.Boardgames.Distinct())
                {
                    if (!IsValid(dto) ||
                        !validBoardgameIds.Contains(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    sellerBoardgames.Add(new BoardgameSeller()
                    {
                        BoardgameId = id
                    });
                }

                sellers.Add(new Seller()
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    Country = dto.Country,
                    Website = dto.Website,
                    BoardgamesSellers = sellerBoardgames
                });

                sb.AppendLine(string.Format(SuccessfullyImportedSeller, 
                    dto.Name, sellerBoardgames.Count));
            }

            context.Sellers.AddRange(sellers);
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
