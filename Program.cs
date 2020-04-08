using System;
using System.IO;
using System.Linq;
using EFCoreConApp.Models;
using EFCoreConApp.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreConApp
{
    class Program
    {
        static IConfiguration BuildConfiguration(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", false, true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
            return configuration;
        }

        // dependency injection in .NET Core does not require external frmaework/packge like Unity/Autofac
        static ServiceProvider ConfigureService(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<NorthWindContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("NWConnection"), sqlServerOptionsAction: sqlOptions =>
          {
              sqlOptions.EnableRetryOnFailure(maxRetryCount: 10,
              maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
          });
            });
            serviceCollection.AddSingleton<NorthWindContext>();

            return serviceCollection.BuildServiceProvider();
        }
        static void Main(string[] args)
        {
            IConfiguration configuration = BuildConfiguration(args);
            ServiceProvider serviceProvider = ConfigureService(configuration);
            var context = serviceProvider.GetService<NorthWindContext>();

            #region Raw queries
            #region regulr query
            // var country = "USA";
            // var customers = context.Customers
            // .FromSqlRaw($"select * from Customers where country = '{country}'").ToList();
            #endregion

            #region SQL parameter
            // var country = new SqlParameter("@country", "USA");
            // var customers = context.Customers
            // .FromSqlRaw($"select * from Customers where country = @country", country).ToList();
            #endregion
            #endregion

            #region All customers
            //var customers = context.Customers.ToList();
            // foreach (var customer in customers)
            // {
            //     Console.WriteLine(customer);
            // }
            #endregion

            #region Get All customers from London
            // var customers = context.Customers.Where(c => c.City == "London")
            //                 .ToList();
            // foreach (var customer in customers)
            // {
            //     Console.WriteLine(customer);
            // }
            #endregion

            #region query expression
            // var customers = from c in context.Customers
            //                 where c.City == "London"
            //                 select new
            //                 {
            //                     c.CustomerID,
            //                     c.CompanyName,
            //                     c.ContactName
            //                 };

            // foreach (var customer in customers)
            // {
            //     Console.WriteLine(customer);
            // }
            #endregion

            #region Get all odres for customer with ID = ALFKI
            // var customers = context.Orders.Where(o => o.CustomerID == "ALFKI").ToList();
            // foreach (var customer in customers)
            // {
            //     Console.WriteLine(customer);
            // }
            #endregion

            #region Get all orders for customer with ID=ALFKI and company name
            // var customers = (from o in context.Orders
            //                 where o.CustomerID == "ALFKI"
            //                 join c in context.Customers on o.CustomerID equals c.CustomerID
            //                 select new {
            //                     o.CustomerID,
            //                     o.ShipCity,
            //                     c.CompanyName
            //                 }).ToList();

            // foreach (var customer in customers)
            // {
            //     Console.WriteLine(customer);
            // }
            #endregion

            #region Get all customers from Lodon with their ID and No. of Orders
            // var customers = (from c in context.Customers
            //                  where c.City == "London"
            //                  join o in context.Orders on c.CustomerID equals o.CustomerID
            //                  group o by o.CustomerID into group1
            //                  select new
            //                  {
            //                      CustomerID = group1.Key,
            //                      NumOfOrders = group1.Count()
            //                  }).ToList();

            // var customers = context.Customers.Where(c => c.City == "London")
            //                 .Join(context.Orders, c => c.CustomerID, o => o.CustomerID,
            //                     (c, o) => new { c, o })
            //                     .GroupBy(c => c.c.CustomerID, o => o.o)
            //                     .Select(g => new { CustomerID = g.Key, NumOrders = g.Count() })
            //                 .ToList();

            // foreach (var customer in customers)
            // {
            //     Console.WriteLine(customer);
            // }
            #endregion

            #region Add new Customer
            // Customer customer1 = new Customer
            // {
            //     CustomerID = "PRM",
            //     CompanyName = "Abhyudaya",
            //     ContactName = "Param",
            //     City = "Goa",
            //     Country = "India"
            // };
            // context.Customers.Add(customer1);
            // try
            // {
            //     var result = context.SaveChanges();
            //     if (result == 1)
            //     {
            //         Console.WriteLine("Customer added successfully");
            //     }
            //     else
            //     {
            //         Console.WriteLine("Add Customer failed");
            //     }
            // }
            // catch (System.Exception ex)
            // {
            //     Console.WriteLine(ex.Message);
            // }

            #endregion

            #region Edit customer, handle concurrency
            // var customer = context.Customers.FirstOrDefault(c => c.CustomerID == "PRM");

            // customer.CompanyName = "Prabhugaonkar Group of companies";

            // try
            // {
            //     var result = context.SaveChanges();
            //     if (result == 1)
            //     {
            //         Console.WriteLine("Customer added successfully");
            //     }
            //     else
            //     {
            //         Console.WriteLine("Add Customer failed");
            //     }
            // }
            // catch (DbUpdateConcurrencyException ex)
            // {
            //     foreach (var item in ex.Entries)
            //     {
            //         if (item.Entity is Customer)
            //         {
            //             var proposedValues = item.CurrentValues;
            //             var databaseValues = item.GetDatabaseValues();

            //             foreach (var property in proposedValues.Properties)
            //             {
            //                 var pValue = proposedValues[property];
            //                 var dValue = databaseValues[property];
            //                 Console.WriteLine($"pValue: {pValue}, dValue: {dValue}");
            //             }

            //             item.OriginalValues.SetValues(databaseValues);
            //             int recordsAffected = context.SaveChanges();
            //             if (recordsAffected == 1)
            //             {
            //                 Console.WriteLine("Updated after concurrency");
            //             }
            //             else
            //             {
            //                 Console.WriteLine("Update failed after concurrency");
            //             }
            //         }
            //     }
            // }
            // catch (System.Exception ex)
            // {
            //     Console.WriteLine(ex.Message);
            // }
            #endregion

            #region delete customer
                
            #endregion
        }
    }
}
