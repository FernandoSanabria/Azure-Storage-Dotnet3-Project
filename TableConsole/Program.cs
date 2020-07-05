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

            CreateTableAsync("testingTable", config["connectionstring"]).Wait();
            Console.WriteLine("it's done");

        }

        private static async Task<CloudTable> CreateTableAsync(string tableName, string connectionString)
        {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
                CloudTable table = tableClient.GetTableReference(tableName);
                if(await table.CreateIfNotExistsAsync())
                {
                    Console.WriteLine("Table {0} created sucessfully", tableName);
                } else {
                    Console.WriteLine("Table {0} exists", tableName);
                }
                Console.WriteLine("async is done");
                return table;
        }
    }
}
