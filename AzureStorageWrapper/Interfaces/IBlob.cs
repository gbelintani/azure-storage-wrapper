namespace AzureStorageWrapper.Interfaces;

public interface IBlob
{
    string Name { get; }
    string Folder { get; }
    Stream Stream { get; }
    string Extension { get; }
    string ContentType { get; }
    IDictionary<string, string>? Metadata { get; }
    IDictionary<string, string>? Tags { get; }
    WrapperBlobType Type { get; }
    string GetFullBlobName();
}