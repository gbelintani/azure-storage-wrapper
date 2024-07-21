namespace AzureStorageWrapper.Interfaces;

public interface IBlobServiceFactory
{
    IBlobWrapperService Create(BlobContainerOptions blobContainerOptions);
}