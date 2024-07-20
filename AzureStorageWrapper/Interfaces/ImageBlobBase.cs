namespace AzureStorageWrapper.Interfaces;

public abstract class ImageBlobBase : BlobBase
{
    public abstract IImageStrategy? ImageStrategy { get; }

    public override string ContentType => $"image/{Extension}";
    public override WrapperBlobType Type => WrapperBlobType.Image;

    protected ImageBlobBase(string name, string extension, Stream stream, string? folder = null) 
        : base(name, extension, stream, folder)
    {
    }
}