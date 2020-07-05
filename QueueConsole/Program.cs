using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace QueueConsole
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


            CloudStorageAccount myClient = CloudStorageAccount.Parse(config["connectionstring"]);
            CloudQueueClient queueClient = myClient.CreateCloudQueueClient();
        }
    }
}
