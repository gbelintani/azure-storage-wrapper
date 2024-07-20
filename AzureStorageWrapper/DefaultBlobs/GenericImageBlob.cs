using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.DefaultBlobs;

public class GenericImageBlob : ImageBlobBase
{
    public override IImageStrategy? ImageStrategy { get; }

    public GenericImageBlob(string name, string extension, Stream stream, string? folder = null,
        IImageStrategy? imageStrategy = null) : base(name, extension, stream, folder)
    {
        ImageStrategy = imageStrategy;
    }
}