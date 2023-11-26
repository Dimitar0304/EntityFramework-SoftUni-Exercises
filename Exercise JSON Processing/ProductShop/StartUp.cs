using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Threading.Channels;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();
            //1 importUsers
            //string userJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context,userJson));

            //2 Import Products
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context,productsJson));

            //3 Import Categories
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context,categoriesJson));

            //4 Import Categories and Products
            //string categoriesProductsJson =
            //    File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context,categoriesProductsJson));

            //5 Export Product In range
            //Console.WriteLine(GetProductsInRange(context));

            //6 Export Users With Sold Products
            //Console.WriteLine(GetSoldProducts(context));

            //7 Export Categories by Product Count
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //8 Export Users and Products
            Console.WriteLine(GetUsersWithProducts(context));
        }
        //1 Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}";
        }
        //2 Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            context.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Length}";
        }
        //3 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);
            var validCategories = categories.Where(c => c.Name is not null).ToArray();

            context.AddRange(validCategories);
            context.SaveChanges();
            return $"Successfully imported {validCategories.Length}";
        }
        //4 Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Length}";
        }

        //5 Export Products In Range

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName,
                })
                .OrderBy(p => p.price)
                .ToList();
            ;
            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            return json;
        }

        //6 Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                
                .Where(u=>u.ProductsSold.Any(p=>p.BuyerId!=null))
                .OrderBy(p=>p.LastName)
                .ThenBy(p=>p.FirstName)
                .Select(u => new
                {
                    
                    firstName= u.FirstName,
                    lastName= u.LastName,
                    soldProducts= u.ProductsSold
                    .Where(p=>p.BuyerId!=null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    }).ToList(),
                })
                
                .ToList();

            var productsJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            return productsJson;

        }

        //7 Export Categories By productCount
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = $"{c.CategoriesProducts.Average(p => p.Product.Price):F2}",
                    totalRevenue = $"{c.CategoriesProducts.Sum(p => p.Product.Price):F2}",
                })
                .OrderByDescending(c => c.productsCount)
                .ToList();

            string categoriesJson = JsonConvert.SerializeObject (categories, Formatting.Indented);
            return categoriesJson;
        }

        //Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold != null && u.ProductsSold.Any(ps => ps.BuyerId != null))
                .Select(u => new
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = u.ProductsSold.Where(p => p.BuyerId != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price
                    })
                    .ToList(),
                })
                .OrderByDescending(p => p.soldProducts.Count())
                .ToList();

            var output = new
            {
                usersCount = users.Count,
                users = users.Select(u => new
                {
                    u.lastName,
                    u.age,
                    soldProducts = new
                    {
                        count = u.soldProducts.Count(),
                        products = u.soldProducts
                        .Select(p => new
                        {
                            p.name,
                            price = p.price.ToString("0.00")
                        })
                    }
                })
            };
            var json = JsonConvert.SerializeObject(output, new JsonSerializerSettings()
            {
                 Formatting = Formatting.Indented,
                 NullValueHandling = NullValueHandling.Ignore,
                 
            });
            
            return json;
        }

    }
}