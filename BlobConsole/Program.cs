using System;
using System.IO;
using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace BlobConsole
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
            Console.WriteLine(getConnString);

            CloudStorageAccount cuentaAlmacenamiento = CloudStorageAccount.Parse(config["connectionstring"]);
            CloudBlobClient clientBlob = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer contenedor = clientBlob.GetContainerReference("contenedorstoragefromdotnet");
            contenedor.CreateIfNotExists();
            contenedor.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            CloudBlockBlob myImageBlobReference = contenedor.GetBlockBlobReference("imageuploadedfromdotnet.png");

            using ( var fileStream = System.IO.File.OpenRead(@"imageuploadedfromdotnet.png"))
            {
                myImageBlobReference.UploadFromStream(fileStream);
            }

            Console.WriteLine("It's done");
            Console.ReadLine();

        }
    }
}
