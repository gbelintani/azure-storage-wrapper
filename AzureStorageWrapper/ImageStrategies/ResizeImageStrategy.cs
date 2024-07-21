using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.ImageStrategies;

/// <summary>
/// ResizeImageStrategy class for resizing an image. If Width or Height is 0, the aspect ratio will be preserved.
/// </summary>
/// <param name="Width"> Width of the image in pixels. 0 for preserving the aspect ratio. </param>
/// <param name="Height"> Height of the image in pixels. 0 for preserving the aspect ratio. </param>
public record ResizeImageStrategy(int Width, int Height) : IImageStrategy
{
}