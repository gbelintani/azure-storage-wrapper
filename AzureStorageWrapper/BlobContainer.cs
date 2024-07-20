namespace AzureStorageWrapper;

public record BlobContainer(string Name)
{
    public bool IsValid() => !string.IsNullOrWhiteSpace(Name);
};