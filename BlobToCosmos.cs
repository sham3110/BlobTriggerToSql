using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class BlobToCosmos
    {
        private readonly ILogger<BlobToCosmos> _logger;

        public BlobToCosmos(ILogger<BlobToCosmos> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BlobToCosmos))]
        [CosmosDBOutput("appdb", "blobinfo", Connection = "CosmosDBConnection", CreateIfNotExists = true )]
        public Object Run([BlobTrigger("data/{name}", Connection = "fd45a9_STORAGE")] BlobClient client, 
            string name)
        {
            _logger.LogInformation($"The name of the blob is : {client.Name}");
            BlobProperties props = client.GetProperties();
            _logger.LogInformation($"The size of the blob  : {props.ContentLength}");
            _logger.LogInformation($"The container of the blob : {client.BlobContainerName}");

            return new {
                id = Guid.NewGuid().ToString(),
                blobname = client.Name,
                bloblength = props.ContentLength,
                blobcontainername = client.BlobContainerName
            };
        }
    }
}
