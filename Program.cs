using System;
using System.IO;
using Microsoft.Extensions.Configuration;

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
        static void Main(string[] args)
        {
            IConfiguration configuration = BuildConfiguration(args);
            Console.WriteLine(configuration["MOD"]);
            Console.WriteLine(configuration.GetConnectionString("NWConnection"));
        }
    }
}
