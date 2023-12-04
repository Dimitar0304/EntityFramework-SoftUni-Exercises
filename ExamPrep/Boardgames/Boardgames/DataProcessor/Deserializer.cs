namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Xml.Serialization;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
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
            XmlRootAttribute root = new XmlRootAttribute("Creators");
            StringBuilder stringBuilder = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCreatorDto[]), root);
            using StringReader reader = new StringReader(xmlString);
            ImportCreatorDto[] creatorsDtos = (ImportCreatorDto[])serializer.Deserialize(reader);

            List<Creator> creatorsToAdd = new List<Creator>();

            foreach (var creatorDto in creatorsDtos)
            {
                if (!IsValid(creatorDto))
                {
                    stringBuilder.AppendLine(ErrorMessage);
                    continue;
                }
                Creator currentCreator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                };
                List<Boardgame> boardgamesToAdd = new List<Boardgame>();
                foreach (var boardgameDto in creatorDto.ImportCreatorBoardgameDtos)
                {
                    if (!IsValid(boardgameDto))
                    {
                        stringBuilder.AppendLine(ErrorMessage);
                        continue;
                    }
                    Boardgame currentBoardgame = new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (CategoryType)boardgameDto.CategoryType,
                        Rating = boardgameDto.Rating,
                        Mechanics = boardgameDto.Mechanics,

                    };
                    boardgamesToAdd.Add(currentBoardgame);
                }
                currentCreator.Boardgames = boardgamesToAdd;
                creatorsToAdd.Add(currentCreator);
                stringBuilder.AppendLine(string.Format(SuccessfullyImportedCreator, currentCreator.FirstName, currentCreator.LastName, boardgamesToAdd.Count));

            }
            context.Creators.AddRange(creatorsToAdd);
            context.SaveChanges();

            return stringBuilder.ToString().Trim();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var sellersDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);
            var sellersToAdd = new List<Seller>();
            foreach (var sellerDto in sellersDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Seller seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Website = sellerDto.Website,
                    Country = sellerDto.Country,
                    Address = sellerDto.Address,

                };
                List<int> uniqueBoardgameIds = new List<int>();
                foreach (var id in sellerDto.BoardgamesIds)
                {
                    if (!uniqueBoardgameIds.Contains(id))
                    {
                        uniqueBoardgameIds.Add(id);
                    }
                }

                List<BoardgameSeller> boardgames = new List<BoardgameSeller>();
                foreach (var id in uniqueBoardgameIds)
                {
                    if (!context.Boardgames.Any(b => b.Id == id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Boardgame boardgame = context.Boardgames.Where(b => b.Id == id).First();

                    BoardgameSeller boardgameSeller = new BoardgameSeller()
                    {
                        Boardgame = boardgame,
                        Seller = seller,
                        BoardgameId = boardgame.Id,
                        SellerId = seller.Id
                    };
                    boardgames.Add(boardgameSeller);
                }
                seller.BoardgamesSellers = boardgames;
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, boardgames.Count));
                sellersToAdd.Add(seller);
            }
            context.Sellers.AddRange(sellersToAdd);
            context.SaveChanges();
                return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
