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
            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Table {0} created sucessfully", tableName);
            }
            else
            {
                Console.WriteLine("Table {0} exists", tableName);
            }

            Console.WriteLine("async is done");

            await InsertOperationAsync(table);
            await RetireveOperationAsync(table);
            await DeleteOperationAsync(table);

            return table;
        }


        private static async Task InsertOperationAsync(CloudTable table)
        {
            Contacto contacto = new Contacto("Chocherita", "Rica")
            {
                Email = "asd@fersa.com",
                Telefono = "23456789"

            };

            TableOperation insertOperation = TableOperation.InsertOrMerge(contacto);
            TableResult result = await table.ExecuteAsync(insertOperation);
            Contacto insertedContact = result.Result as Contacto;
            Console.WriteLine("contact added");
        }


        private static async Task RetireveOperationAsync(CloudTable table)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Contacto>("Flora", "Valle");
            TableResult result = await table.ExecuteAsync(retrieveOperation);
            Contacto customer = result.Result as Contacto;

            if (customer != null)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", customer.PartitionKey, customer.RowKey, customer.Email, customer.Telefono);
            }
        }

        private static async Task DeleteOperationAsync(CloudTable table) 
        {
            Contacto contacto = new Contacto("Fer", "Stein")
            {
                Email = "asd@fersa.com",
                Telefono = "4567890873878",
                ETag = "*"
            };
 
            TableOperation deleteOperation = TableOperation.Delete(contacto);
            TableResult result = await table.ExecuteAsync(deleteOperation);
 
            Console.WriteLine("Resultado de la operación de eliminado: " + result.RequestCharge);
        }


    }
}
