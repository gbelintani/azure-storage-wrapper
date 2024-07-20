using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.DefaultBlobs;

public class GenericFileBlob : FileBlobBase
{
    public GenericFileBlob(string name, string extension, Stream stream, string? folder = null) 
        : base(name, extension, stream, folder)
    {
    }
}