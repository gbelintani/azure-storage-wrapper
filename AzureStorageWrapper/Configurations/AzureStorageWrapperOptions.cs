using AzureStorageWrapper.Interfaces;

namespace AzureStorageWrapper.Configurations;

public class AzureStorageWrapperOptions
{
    private readonly IDictionary<WrapperBlobType, ContainerConfiguration> _containers =
        new Dictionary<WrapperBlobType, ContainerConfiguration>
        {
            { WrapperBlobType.File, new ContainerConfiguration("docs", WrapperBlobType.File) },
            { WrapperBlobType.Image, new ContainerConfiguration("assets", WrapperBlobType.Image) }
        };

    public string? ConnectionString { get; set; }

    public void RegisterContainer(WrapperBlobType type, string name)
    {
        _containers[type] = new ContainerConfiguration(name, type);
    }

    public string GetContainer(WrapperBlobType type)
    {
        if (!_containers.TryGetValue(type, out var container))
            throw new ArgumentException($"No container for type {type} defined");

        return container.Name;
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(ConnectionString);
    }
}