using Cadastre.Data;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            var date = DateTime.ParseExact("01/01/2000","dd/MM/yyyy",CultureInfo.InvariantCulture);
            var properties = dbContext.Properties
                 .Where(p =>p.DateOfAcquisition>=date)
                 .ToArray()
                 .OrderByDescending(p => p.DateOfAcquisition)
                 .ThenBy(p => p.PropertyIdentifier)
                 .Select(p => new
                 {
                     PropertyIdentifier = p.PropertyIdentifier,
                     Area = p.Area,
                     Address= p.Address,
                     DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                     Owners = p.PropertiesCitizens
                     .Select(pc => new
                     {
                         LastName = pc.Citizen.LastName,
                         MaritalStatus = pc.Citizen.MaritalStatus.ToString()
                     })
                     .OrderBy(pc => pc.LastName)
                     .ToList()
                 })
                  .ToList();

            return JsonConvert.SerializeObject(properties,Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            throw new NotImplementedException();
        }
    }
}
