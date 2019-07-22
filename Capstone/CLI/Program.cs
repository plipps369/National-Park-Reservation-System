using Capstone.DAL;
using Capstone.Models;
using Microsoft.Extensions.Configuration;
using Home;
using System;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            

            string connectionString = configuration.GetConnectionString("Project");
            IDAO DAO = new DbDAO(connectionString);
            ParkIDAO parkDAO = new ParkDAO(connectionString);
          
            CLI cli = new CLI(DAO, parkDAO);
            cli.ShowMain();
        }
    }
}
