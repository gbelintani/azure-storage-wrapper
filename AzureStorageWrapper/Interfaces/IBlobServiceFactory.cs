namespace AzureStorageWrapper.Interfaces;

public interface IBlobServiceFactory
{
    IBlobWrapperService Create(BlobContainer blobContainer);
}