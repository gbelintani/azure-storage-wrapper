namespace AzureStorageWrapper.Interfaces;

/// <summary>
/// Base class for Blob
/// </summary>
public abstract class BlobBase : IBlob
{
    /// <summary>
    /// Name of the blob
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Folder of the blob. Can be a path or a name of a folder
    /// </summary>
    public string? Folder { get; set; }

    /// <summary>
    /// Stream of the blob
    /// </summary>
    public Stream Stream { get; set; }

    /// <summary>
    /// Extension of the blob. Will be used to identify the content type
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// Content type of the blob
    /// </summary>
    public virtual string ContentType { get; set; }

    /// <summary>
    /// Metadata of the blob
    /// </summary>
    public IDictionary<string, string>? Metadata { get; set; }

    /// <summary>
    /// Tags of the blob
    /// </summary>
    public IDictionary<string, string>? Tags { get; set; }

    /// <summary>
    /// Type of the blob
    /// </summary>
    public virtual WrapperBlobType Type { get; }

    protected BlobBase(string name, string extension, Stream stream, string? folder = null)
    {
        Name = name;
        Extension = extension;
        Stream = stream;
        Folder = folder ?? "";
    }

    /// <summary>
    /// Gets the full name of the blob. If the folder is not set, the name will be returned as is. Otherwise, the folder and the name will be concatenated.
    /// </summary>
    /// <returns>The full name of the blob.</returns>
    public virtual string GetFullBlobName()
    {
        if (string.IsNullOrEmpty(Folder))
        {
            return $"{Name}.{Extension}";
        }

        return $"{Folder}/{Name}.{Extension}";
    }
}