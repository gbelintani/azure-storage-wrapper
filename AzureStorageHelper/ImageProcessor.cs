using AzureStorageHelper.ImageStrategies;
using AzureStorageHelper.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using ImageExtensions = AzureStorageHelper.Interfaces.ImageExtensions;

namespace AzureStorageHelper
{
    public static class ImageProcessor
    {
        public static async Task<byte[]> ProcessAsync(IImageBlob imageBlob)
        {
            var strategy = imageBlob.ImageStrategy ?? new DefaultImageStrategy();
            
            return await ApplyStrategyAsync(imageBlob, strategy);
        }

        private static async Task<byte[]> ApplyStrategyAsync(IImageBlob imageBlob, IImageStrategy strategy)
        {
            var image = Image.Load(imageBlob.Stream);
            image.Mutate(x => x.Resize(strategy.Width, strategy.Height, true));
            var memoryStream = new MemoryStream();
            switch (imageBlob.ImageExtension)
            {
                case ImageExtensions.gif:
                    await image.SaveAsGifAsync(memoryStream);
                    break;
                default:
                    await image.SaveAsPngAsync(memoryStream);
                    break;
            }
            return memoryStream.ToArray();
        }
    }
}
