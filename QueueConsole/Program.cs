using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Azure.Storage.Blob;

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

            CloudQueue queue = queueClient.GetQueueReference("filaprocesos");
            // queue.CreateIfNotExists();

            // for (int i = 0; i < 500; i++)
            // {
            //     CloudQueueMessage message = new CloudQueueMessage(string.Format("Operacion: {0}", i));
            //     queue.AddMessage(message);

            //     Console.WriteLine(i.ToString() + " Mensaje publicado");
            // }
            // Console.ReadLine();

            CloudQueueMessage peekedMessage = queue.PeekMessage();

            CloudBlobClient blobClient = myClient.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("contenedorqueues");
            container.CreateIfNotExists();


            foreach (CloudQueueMessage item in queue.GetMessages(20, TimeSpan.FromSeconds(100)))
            {
                string filePath = string.Format(@"log{0}.txt", item.Id);
                TextWriter tempFile = File.CreateText(filePath);
                var message = queue.GetMessage().AsString;
                tempFile.WriteLine(message);
                Console.WriteLine("File created");
                tempFile.Close();


                using (var fileStream = System.IO.File.OpenRead(filePath))
                {
                    CloudBlockBlob myBlob = container.GetBlockBlobReference(string.Format(@"log{0}.txt", item.Id));
                    myBlob.UploadFromStream(fileStream);
                    Console.WriteLine("Blob created");
                }

                queue.DeleteMessage(item);

            }
            Console.ReadLine();
        }
    }
}
