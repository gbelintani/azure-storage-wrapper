namespace AzureStorageWrapper.Interfaces;

public abstract class ImageBlobBase : BlobBase
{
    /// <summary>
    /// Image strategy to be used for processing the image. Can be null for no processing.
    /// </summary>
    public abstract IImageStrategy? ImageStrategy { get; }

    /// <summary>
    /// Content type of the blob. Defaults to image/extension.
    /// </summary>
    public override string ContentType => $"image/{Extension}";
    
    /// <summary>
    /// Type of the blob. Defaults to WrapperBlobType.Image.
    /// </summary>
    public override WrapperBlobType Type => WrapperBlobType.Image;

    protected ImageBlobBase(string name, string extension, Stream stream, string? folder = null) 
        : base(name, extension, stream, folder)
    {
    }
}