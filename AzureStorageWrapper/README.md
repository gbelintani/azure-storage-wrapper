# Azure Storage Wrapper

This is a simple wrapper for Azure Storage. For simple use cases you can use this in a simpler way than the official
Azure Storage SDK.
It does the instantiation of the clients and abstracts the calls to the basic operations like uploading, downloading and
deleting. It also simplifies tagging, adding metadata and setting the content type.

There's 2 services available:

- File Blob Storage
  Which manages files in a container

- Image Blob Storage
  Which can resize images based on an ImageStrategy and store them in a container

## Usage

Add the connection string to your appsettings.json or secrets as follows:

```json
{
    "BlobStorageConfiguration:BlobConnectionString": "YOUR_BLOB_CONNECTION_STRING"
}
```

### File Blob Storage

Inject as dependency in your Startup.cs

```csharp
services.AddScoped<FileBlobService>();
```

Make a class that implements IFileBlob, instantiate it and use the service

```csharp
public class GenericFile : IFileBlob
{
    public string Name { get; }
    public string Folder { get; }
    public Stream Stream { get; }
    public string Extension { get; }
    public string ContentType { get; }
    public IDictionary<string, string> Metadata { get; } = new Dictionary<string, string>();
    public IDictionary<string, string> Tags { get; } = new Dictionary<string, string>();
}


var file = new GenericFile()
{
    Name = "test",
    Folder = "test",
    Stream = stream,
    Extension = "txt"
};

//Upload
var url = await _fileBlobService.Upload(file);

//Delete by Tag
await _fileBlobService.Delete(new KeyValuePair<string, string>("ID","1234"));

//Get By Tag
var blobNames = await _fileBlobService.GetByTag(new KeyValuePair<string, string>("ID", "1234"));

//Download
var downloadStream = await _fileBlobService.GetFileStream(blobNames.First().Name);
```

### Image Blob Storage

Inject as dependency in your Startup.cs

```csharp
services.AddScoped<ImageBlobService>();
```

Make a class that implements IImageStrategy

```csharp
public class AvatarProfileStrategy : IImageStrategy
{
    public int Width => 128;
    public int Height => 128; //0 is auto
}
```

Make a class that implements IImageBlob and use the service

```csharp
public class AvatarProfileImage : IImageBlob
{
    public IImageStrategy? ImageStrategy { get; } = AvatarProfileStrategy(); //null for no resizing
    public string Name { get; }
    public string Folder { get; }
    public Stream Stream { get; }
    public string Extension { get; }
    public string ContentType { get; }
    public IDictionary<string, string> Metadata { get; } = new Dictionary<string, string>();
    public IDictionary<string, string> Tags { get; } = new Dictionary<string, string>();
}

var image = new AvatarProfileImage()
{
    Name = "test.png",
    Folder = "test",
    Stream = stream,
    Extension = "png"
};

//Upload
var url = await _imageBlobService.Upload(image);
```
