# Azure Storage Wrapper

This is a simple wrapper for Azure Storage. The main motivation for this was to have a simpler way to upload images with resizing options to Azure Storage.
For simple use cases you can use this in a simpler way than the official Azure Storage SDK.
It does the instantiation of the clients and abstracts the calls to the basic operations like uploading, downloading and
deleting. It also simplifies tagging, adding metadata and setting the content type.


## Usage

Add the connection string to your appsettings.json or secrets as follows:

```json
{
    "ConnectionStrings:BlobDefaultConnection": "YOUR_BLOB_CONNECTION_STRING"
}
```

Inject as dependency in your Startup.cs

```csharp
services.AddAzureStorageWrapper(config.GetConnectionString("BlobDefaultConnection"));
```

### Files

You can use the service like this:

```csharp

private readonly IBlobWrapperService _blobServiceWrapper;

public BlobSandbox(IBlobServiceFactory blobServiceFactory)
{
    _blobServiceWrapper = blobServiceFactory.Create(new BlobContainerOptions("WrapperTest", PublicAccessType.Blob));
}

//Upload
var blobText = new GenericFileBlob("test", "txt", File.OpenRead("Resources\\test.txt"), "AzureStorageWrapper");
var urlText = await _blobServiceWrapper.Upload(blobText);

//Delete by Tag
await _blobServiceWrapper.Delete(new KeyValuePair<string, string>("ID","1234"));

//Get By Tag
var blobNames = await _blobServiceWrapper.GetByTag(new KeyValuePair<string, string>("ID", "1234"));

//Download
var downloadStream = await _blobServiceWrapper.GetFileStream(blobNames.First().Name);
```

### Images


```csharp
private readonly IBlobWrapperService _blobServiceWrapper;

public BlobSandbox(IBlobServiceFactory blobServiceFactory)
{
    _blobServiceWrapper = blobServiceFactory.Create(new BlobContainerOptions("WrapperTest", PublicAccessType.Blob));
}

//Resize and Upload 
var blobImg = new GenericImageBlob("test", "gif", File.OpenRead("Resources\\test.gif"), "AzureStorageWrapper", new ResizeImageStrategy(200, 0));
var urlText = await _blobServiceWrapper.Upload(blobText);
```

