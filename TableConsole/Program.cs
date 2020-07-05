using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace TableConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string getConnString = config["connectionstring"];
        }

        private static async Task<CloudTable> CreateTableAsync(string tableName, string connectionString)
        {

        }
    }
}
