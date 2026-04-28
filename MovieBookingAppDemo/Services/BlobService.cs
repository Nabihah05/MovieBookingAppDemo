using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MovieBookingAppDemo.Service
{
    public class BlobService
    {
        private readonly string _connectionString;

        //constructor 
        public BlobService(IConfiguration config)
        {
            //gets the connection string from appsettings.json
            _connectionString = config.GetConnectionString("BlobStorage")!;
        }


        //uploads file to local blob storage and returns the image URL 
        public async Task<string?> UploadFileAsync(IFormFile file, string containerName)
        {
            if (file == null || file.Length == 0)
                return null;

            //Creates a connection to the local blob storage 
            var blobServiceClient = new BlobServiceClient(_connectionString);

            //access the container where files (i.e images) will be stored 
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // if the container doesnt exist, create it 
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            // PublicAccessType.Blob allows the browser to display the image

            //gets file extension (.jpg, .png .jpeg etc)
            var extension = Path.GetExtension(file.FileName);

            //creates a unique name for each file using GUID (avoids duplication)
            var blobName = $"{Guid.NewGuid()}{extension}";

            // // Get a reference to the blob (file) inside the container
            var blobClient = containerClient.GetBlobClient(blobName);

            // Open the file stream and upload it to blob storage
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            // Return the URL of the uploaded file
            // URL is saved in the database (as a string)
            return blobClient.Uri.ToString();
        }

    }
}
