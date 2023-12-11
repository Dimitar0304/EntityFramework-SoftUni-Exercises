namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Microsoft.Data.SqlClient.Server;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute root = new XmlRootAttribute("Districts");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportDistrictDto[]), root);
            StringReader reader = new StringReader(xmlDocument);

            ImportDistrictDto[] districtsDtos = (ImportDistrictDto[])serializer.Deserialize(reader);
            var districtsToAdd = new List<District>();

            foreach (var disDto in districtsDtos)
            {
                if(!IsValid(disDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                int regionIndex = 0;
                switch (disDto.Region)
                {
                    case "SouthEast":
                        regionIndex = 0;
                        break;
                    case "SouthWest":
                        regionIndex = 1;
                        break;
                    case "NorthEast":
                        regionIndex = 2;
                        break;
                    case "NorthWest":
                        regionIndex = 3;
                        break;
                    default:
                        sb.AppendLine(ErrorMessage);
                        continue;
                        
                }

                District district = new District()
                {
                    Name = disDto.Name,
                    PostalCode = disDto.PostalCode,
                    Region = (Region)regionIndex    
                };
                if (districtsToAdd.Any(d=>d.Name==disDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var propertiesToAdd = new List<Property>();
                foreach (var pDto in disDto.Properties)
                {
                    CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                    if (!IsValid(pDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Property property = new Property()
                    {
                        PropertyIdentifier = pDto.PropertyIdentifier,
                        Address = pDto.Address,
                        Area = pDto.Area,
                        Details = pDto.Details,
                        DateOfAcquisition = DateTime.ParseExact(pDto.DateOfAcquisition, "dd/MM/yyyy",cultureInfo)
                    };
                    if (propertiesToAdd.Any(p=>p.PropertyIdentifier==property.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (propertiesToAdd.Any(p => p.Address == property.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    propertiesToAdd.Add(property);
                }
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict, disDto.Name, propertiesToAdd.Count()));
                district.Properties = propertiesToAdd;
                districtsToAdd.Add( district );
            }
            dbContext.Districts.AddRange(districtsToAdd);
            dbContext.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();
            var citizensDtos = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);
            var citizensToAdd = new List<Citizen>();
            foreach (var dto in citizensDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                int martialStatusIndex = 0;
                switch (dto.MaritalStatus)
                {
                    case "Unmarried":
                        martialStatusIndex = 0;
                        break;
                    case "Married":
                        martialStatusIndex = 1;
                        break;
                    case "Divorced":
                        martialStatusIndex = 2;
                        break;
                    case "Widowed":
                        martialStatusIndex = 3;

                        break;
                    default:
                        sb.AppendLine(ErrorMessage);
                        continue;
                        
                }

                Citizen citizen = new Citizen()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    MaritalStatus = (MaritalStatus)martialStatusIndex,
                    BirthDate = DateTime.ParseExact(dto.BirthDate,"dd-MM-yyyy",CultureInfo.InvariantCulture)
                };
                List<PropertyCitizen> propertyCitizens = new List<PropertyCitizen>();
                foreach (var id in dto.PropertiesIds)
                {
                    if (dbContext.Properties.Any(p=>p.Id==id))
                    {
                        Property property = dbContext.Properties.Where(p => p.Id == id).First();
                    PropertyCitizen propertyCitizen = new PropertyCitizen()
                    {
                        PropertyId = property.Id,
                        Property = property,
                        Citizen = citizen,
                        CitizenId = citizen.Id
                    };
                        propertyCitizens.Add(propertyCitizen);
                    }
                }
                citizen.PropertiesCitizens = propertyCitizens;
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count()));
                citizensToAdd.Add(citizen);
            }
            dbContext.AddRange(citizensToAdd);
            dbContext.SaveChanges();
            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
