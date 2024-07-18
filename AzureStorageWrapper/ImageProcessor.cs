using AzureStorageWrapper.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AzureStorageWrapper;

public static class ImageProcessor
{
    public static async Task<byte[]?> ProcessAsync(ImageBlobBase imageBlob)
    {
        if (imageBlob.ImageStrategy == null) return null;

        return await ApplyStrategyAsync(imageBlob, imageBlob.ImageStrategy);
    }

    private static async Task<byte[]> ApplyStrategyAsync(ImageBlobBase imageBlob, IImageStrategy strategy)
    {
        var image = await Image.LoadAsync(imageBlob.Stream);
        image.Mutate(x => x.Resize(strategy.Width, strategy.Height, true));
        var memoryStream = new MemoryStream();
        switch (imageBlob.Extension)
        {
            case "gif":
                await image.SaveAsGifAsync(memoryStream);
                break;
            default:
                await image.SaveAsPngAsync(memoryStream);
                break;
        }

        return memoryStream.ToArray();
    }
}