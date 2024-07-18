using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.ImageStrategies;

public record DefaultImageStrategy(int Width, int Height) : IImageStrategy
{
}