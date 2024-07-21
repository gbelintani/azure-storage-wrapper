using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.Configurations;

/// <summary>
/// Options for AzureStorageWrapper
/// </summary>
public class AzureStorageWrapperOptions
{
    /// <summary>
    /// Connection string to the Azure Storage Account. You can use the Azure Portal to get the connection string.
    /// </summary>
    public string? ConnectionString { get; set; }

    
    /// <summary>
    /// Validates the options
    /// </summary>
    /// <returns> True if the options are valid, false otherwise. </returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(ConnectionString);
    }
}