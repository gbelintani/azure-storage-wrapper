using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.DefaultBlobs;

public class GenericImageBlob : ImageBlobBase
{
    public GenericImageBlob()
    {
    }

    public GenericImageBlob(IImageStrategy imageStrategy)
    {
        ImageStrategy = imageStrategy;
    }

    public override IImageStrategy? ImageStrategy { get; }
}