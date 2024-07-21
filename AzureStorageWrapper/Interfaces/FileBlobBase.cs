namespace AzureStorageWrapper.Interfaces;

public abstract class FileBlobBase : BlobBase
{
    /// <summary>
    /// Content type of the blob. Defaults to application/octet-stream, but will be overwritten if there's a specific content type for the blob based on the extension.
    /// </summary>
    public override string ContentType => "application/octet-stream";

    /// <summary>
    /// Type of the blob. Defaults to WrapperBlobType.File.
    /// </summary>
    public override WrapperBlobType Type => WrapperBlobType.File;

    protected FileBlobBase(string name, string extension, Stream stream, string? folder = null)
        : base(name, extension, stream, folder)
    {
    }
}