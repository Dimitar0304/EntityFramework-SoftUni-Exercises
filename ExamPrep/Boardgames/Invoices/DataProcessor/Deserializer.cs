namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Clients");

            XmlSerializer serializer = new XmlSerializer(typeof(ImportClientDto[]),root);
            StringReader reader = new StringReader(xmlString);
            ImportClientDto[] clientsDtos = (ImportClientDto[])serializer.Deserialize(reader);

            List<Client> clients = new List<Client>();

            foreach (var dto in clientsDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client currentClient = new Client()
                {
                    Name = dto.Name,
                    NumberVat = dto.NumberVat,
                };
                clients.Add(currentClient);
                foreach (var addresDto in dto.ImportAddressesDtos)
                {
                    List<Address> addresses = new List<Address>();
                    if (!IsValid(addresDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Address currentAddress =new Address()
                    {
                        StreetName = addresDto.StreetName,
                        StreetNumber = addresDto.StreetNumber,
                        Country = addresDto.Country,
                        City =  addresDto.City,
                        PostCode = addresDto.PostCode,
                    };
                    currentClient.Addresses.Add(currentAddress);
                    context.Addresses.Add(currentAddress);

                }
                sb.AppendLine(string.Format(SuccessfullyImportedClients,currentClient.Name));
                
            }
            context.Clients.AddRange(clients);
            context.SaveChanges();
            return sb.ToString().Trim();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            var invoicesDtos = JsonConvert.DeserializeObject<ImportInvoicesDto[]>(jsonString);
            var invoicesToAdd = new List<Invoice>();

            StringBuilder sb = new StringBuilder();
            foreach (var iDto in invoicesDtos)
            {
                if (!IsValid(iDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var issueDate = DateTime.Parse(iDto.IssueDate);
                var dueDate = DateTime.Parse(iDto.DueDate);
                if (dueDate< issueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice invoice = new Invoice()
                {
                    Number = iDto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    CurrencyType = (CurrencyType)iDto.CurrencyType,
                    Amount = iDto.Amount,
                    ClientId = iDto.ClientId,
                };
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices,invoice.Number));
                invoicesToAdd.Add(invoice);
            }
            context.Invoices.AddRange(invoicesToAdd);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            var productsDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Product> products = new List<Product>();


            foreach (var pDto in productsDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var uniqueClients = pDto.ClientIds.Distinct();
                List<int> validClientIds = new List<int>();
                foreach (var id in uniqueClients)
                {
                    if (context.Clients.Any(c=>c.Id==id))
                    {
                        validClientIds.Add(id);
                    }
                    else
                    {
                        sb.AppendLine(ErrorMessage);
                    }
                }

                Product currentProduct = new Product()
                {
                    Name = pDto.Name,
                    CategoryType = (CategoryType)pDto.CategoryType,
                    Price = pDto.Price,

                };

                List<ProductClient> clients = new List<ProductClient>();
                foreach (var id in validClientIds)
                {
                    ProductClient productClient = new ProductClient()
                    {
                        ProductId = currentProduct.Id,
                        ClientId = id,
                    };
                    clients.Add(productClient);
                }
                currentProduct.ProductsClients = clients;
                sb.AppendLine(string.Format(SuccessfullyImportedProducts,currentProduct.Name,clients.Count()));
                products.Add(currentProduct);
            }
            context.Products.AddRange(products);
            context.SaveChanges();
            return sb.ToString().Trim();

        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
