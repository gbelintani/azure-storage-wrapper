using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.DefaultBlobs;

/// <summary>
/// GenericFileBlob class for creating a generic file blob.
/// </summary>
public class GenericFileBlob : FileBlobBase
{
    /// <summary>
    /// Creates a new instance of the GenericFileBlob class.
    /// </summary>
    /// <param name="name"> Name of the blob. </param>
    /// <param name="extension"> Extension of the blob. Will be used to identify the content type. </param>
    /// <param name="stream"> Stream of the blob. </param>
    /// <param name="folder"> Folder of the blob. Can be a path or a name of a folder. Or null if the blob is in the root folder. </param>
    public GenericFileBlob(string name, string extension, Stream stream, string? folder = null) 
        : base(name, extension, stream, folder)
    {
    }
}