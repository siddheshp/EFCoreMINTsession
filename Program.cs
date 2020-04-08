using System;
using System.IO;
using System.Linq;
using EFCoreConApp.Models;
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

            return  serviceCollection.BuildServiceProvider();
        }
        static void Main(string[] args)
        {
            IConfiguration configuration = BuildConfiguration(args);
            ServiceProvider serviceProvider = ConfigureService(configuration);
            var context = serviceProvider.GetService<NorthWindContext>();

            #region All customers
            var customers  = context.Customers.ToList();    
            foreach (var customer in customers)
            {
                Console.WriteLine(customer);
            }
            #endregion
        }
    }
}
