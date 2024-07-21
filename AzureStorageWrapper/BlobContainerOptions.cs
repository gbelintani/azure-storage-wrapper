using Azure.Storage.Blobs.Models;

namespace AzureStorageWrapper;

/// <summary>
/// BlobContainerOptions class for specifying the name and default public access type of the blob container.
/// </summary>
/// <param name="Name"> Name of the blob container. This will be converted to lowercase. </param>
/// <param name="DefaultPublicAccessType">If the blob container does not exist, the default public access type will be used. Defaults to PublicAccessType.None.</param>
public record BlobContainerOptions(string Name, PublicAccessType DefaultPublicAccessType = PublicAccessType.None)
{
    public string GetFormattedName() => Name.ToLower();

    public bool IsValid() => !string.IsNullOrWhiteSpace(Name);
};