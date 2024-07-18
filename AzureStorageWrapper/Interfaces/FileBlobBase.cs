namespace AzureStorageWrapper.Interfaces;

public abstract class FileBlobBase : BlobBase
{
    public override string ContentType => "application/octet-stream";
    public override WrapperBlobType Type => WrapperBlobType.File;
}