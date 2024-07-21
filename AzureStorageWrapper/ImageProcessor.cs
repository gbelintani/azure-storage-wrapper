using AzureStorageWrapper.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AzureStorageWrapper;

public static class ImageProcessor
{
    /// <summary>
    /// Processes an image blob using the specified strategy.
    /// </summary>
    /// <param name="imageBlob"> Image blob to be processed. </param>
    /// <param name="ct"> Cancellation token. </param>
    /// <returns> A byte array representing the processed image. </returns>
    public static async Task<byte[]?> ProcessAsync(ImageBlobBase imageBlob, CancellationToken ct = default)
    {
        if (imageBlob.ImageStrategy == null) return null;

        return await ApplyStrategyAsync(imageBlob, imageBlob.ImageStrategy, ct);
    }

    private static async Task<byte[]> ApplyStrategyAsync(ImageBlobBase imageBlob, IImageStrategy strategy,
        CancellationToken ct = default)
    {
        var image = await Image.LoadAsync(imageBlob.Stream, ct);
        image.Mutate(x => x.Resize(strategy.Width, strategy.Height, true));
        var memoryStream = new MemoryStream();
        switch (imageBlob.Extension)
        {
            case "gif":
                await image.SaveAsGifAsync(memoryStream, ct);
                break;
            default:
                await image.SaveAsPngAsync(memoryStream, ct);
                break;
        }

        return memoryStream.ToArray();
    }
}