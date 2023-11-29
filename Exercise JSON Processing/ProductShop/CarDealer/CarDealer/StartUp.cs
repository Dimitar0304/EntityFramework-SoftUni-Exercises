using AutoMapper;
using AutoMapper.Internal;
using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Runtime.ExceptionServices;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();
            //9 Import Suppliers
            //string suplilersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context,suplilersJson));

            // 10 Import Parts
            //string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, partsJson));

            //11 Import cars 
            //string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(context, carsJson));

            //Import Customers
            //string customerJson = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context,customerJson));

            //13 Import Sales
            //string salesJson = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(context,salesJson));

            //14 Export Ordered Customers
            //Console.WriteLine(GetOrderedCustomers(context));

            //15 Export Toyota Cars
            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //16 Get NoImportedSuppliers
            //Console.WriteLine(GetLocalSuppliers(context));

            //17 Get all cars and their parts
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //18 Get total Sales by customers
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            //19 Get first 10 sales
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }
        //9 Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var supplires = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(supplires);
            context.SaveChanges();
            return $"Successfully imported {supplires.Length}.";
        }

        // 10 Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson);
            int counter = 0;
            foreach (var p in parts)
            {
                if (context.Suppliers.Any(s => s.Id == p.SupplierId))
                {
                    context.Parts.Add(p);
                    counter++;
                }
            }


            context.SaveChanges();
            return $"Successfully imported {counter}.";
        }
        //11 Import cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);
            CarsDto[] importCarDtos = JsonConvert.DeserializeObject<CarsDto[]>(inputJson);

            //Mapping the Cars from their DTOs
            ICollection<Car> carsToAdd = new HashSet<Car>();

            foreach (var carDto in importCarDtos)
            {
                Car currentCar = mapper.Map<Car>(carDto);

                foreach (var id in carDto.PartsIds)
                {
                    if (context.Parts.Any(p => p.Id == id))
                    {
                        currentCar.PartsCars.Add(new PartCar
                        {
                            PartId = id,
                        });
                    }
                }

                carsToAdd.Add(currentCar);
            }

            //Adding the Cars
            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            //Output
            return $"Successfully imported {carsToAdd.Count}.";
        }

        //12 Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);
            context.Customers.AddRange(customers);

            context.SaveChanges();
            return $"Successfully imported {customers.Length}.";
        }
        //13 Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }

        //Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();
            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json;
        }
        //15
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaModels = context.Cars.Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                })
                .ToArray();
            string json = JsonConvert.SerializeObject(toyotaModels, Formatting.Indented);
            return json;
        }
        //16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliersThatNoHaveParts = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                });

            var json = JsonConvert.SerializeObject(suppliersThatNoHaveParts, Formatting.Indented);
            return json;
        }

        //17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance,
                    },
                    parts = c.PartsCars.Select(s => new
                    {
                        Name = s.Part.Name,
                        Price = s.Part.Price.ToString("f2")
                    }).ToArray()
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return json;

        }
        //18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersThatBuyCar = context.Customers
                .Where(c => c.Sales.Count() > 0)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count(),
                    spentMoney = c.Sales.Sum(car => car.Car.PartsCars.Sum(p => p.Part.Price))
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToList();

            string json = JsonConvert.SerializeObject(customersThatBuyCar, Formatting.Indented);
            return json;
        }

        //19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var first10Sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Sum(pc => pc.Part.Price).ToString("f2"),
                    priceWithDiscount = ((s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - s.Discount / 100))).ToString("f2")
                })
                .ToArray();
            return JsonConvert.SerializeObject(first10Sales, Formatting.Indented);
        }
    }
}