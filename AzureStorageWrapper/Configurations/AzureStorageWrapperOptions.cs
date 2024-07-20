using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.Configurations;

public class AzureStorageWrapperOptions
{
    public string? ConnectionString { get; set; }

    
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(ConnectionString);
    }
}