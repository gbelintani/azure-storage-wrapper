using Azure.Storage.Blobs.Models;

namespace AzureStorageWrapper;

public record BlobContainer(string Name, PublicAccessType DefaultPublicAccessType = PublicAccessType.None)
{
    public string GetFormattedName() => Name.ToLower();
    
    public bool IsValid() => !string.IsNullOrWhiteSpace(Name);
};