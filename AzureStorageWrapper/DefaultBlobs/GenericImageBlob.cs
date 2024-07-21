using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.DefaultBlobs;

/// <summary>
/// GenericImageBlob class for creating a generic image blob.
/// </summary>
public class GenericImageBlob : ImageBlobBase
{
    public override IImageStrategy? ImageStrategy { get; }

    /// <summary>
    /// Creates a new instance of the GenericImageBlob class.
    /// </summary>
    /// <param name="name"> Name of the blob. </param>
    /// <param name="extension"> Extension of the blob. Will be used to identify the content type. </param>
    /// <param name="stream"> Stream of the blob. </param>
    /// <param name="folder"> Folder of the blob. Can be a path or a name of a folder. Or null if the blob is in the root folder. </param>
    /// <param name="imageStrategy"> Image strategy to be used for processing the image. Can be null for no processing. </param>
    public GenericImageBlob(string name, string extension, Stream stream, string? folder = null,
        IImageStrategy? imageStrategy = null) : base(name, extension, stream, folder)
    {
        ImageStrategy = imageStrategy;
    }
}