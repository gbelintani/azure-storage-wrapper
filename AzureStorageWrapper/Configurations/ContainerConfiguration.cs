using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.Configurations;

public record ContainerConfiguration(string Name, WrapperBlobType Type)
{
}