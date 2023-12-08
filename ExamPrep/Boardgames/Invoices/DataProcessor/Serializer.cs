namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            throw new NotImplementedException();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            var products = context.Products
                .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length > nameLength)&&p.ProductsClients.Any(pc=>pc.ClientId!=null))
                .Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price.ToString(":F2"),
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients
                    .Where(pc => pc.Client.Name.Length > nameLength)
                    .Select(c => new
                    {
                        Name = c.Client.Name,
                        NumberVat = c.Client.NumberVat
                    })
                    .OrderBy(c => c.Name)
                    .ToList()
                })
                .OrderByDescending
             (p=>p.Clients.Count())
             .ThenBy(p=>p.Name)
                .Take(5)
             .ToList();

            return JsonConvert.SerializeObject(products);
        }
    }
}