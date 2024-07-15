using AzureStorageHelper.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureStorageHelper
{
    public class ImageBlobService : BlobServiceBase<IImageBlob>
    {
        private const string IMAGE_CONTAINER = "assets";

        public ImageBlobService(ILogger<ImageBlobService> logger, IConfiguration configuration) : base(logger,
            configuration, IMAGE_CONTAINER)
        {
        }

        public override async Task<string> Upload(IImageBlob blobInfo)
        {
            byte[] finalImage;
            if (blobInfo.ImageStrategy != null)
            {
                _logger.LogDebug($"Image will be processed and saved for {blobInfo.Name}");
                finalImage = await ImageProcessor.ProcessAsync(blobInfo);
                _logger.LogDebug($"Image processed for {blobInfo.Name}");
            }
            else
            {
                var streamEnd = Convert.ToInt32(blobInfo.Stream.Length);
                var buffer = new byte[streamEnd];
                await blobInfo.Stream.ReadAsync(buffer, 0, streamEnd);
                finalImage = buffer;
            }

            return await UploadToBlobContainer(blobInfo, finalImage);
        }
    }
}