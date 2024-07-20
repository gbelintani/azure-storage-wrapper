// See https://aka.ms/new-console-template for more information

using Azure.Storage.Blobs.Models;
using AzureStorageWrapper;
using AzureStorageWrapper.Configurations;
using AzureStorageWrapper.DefaultBlobs;
using AzureStorageWrapper.ImageStrategies;
using AzureStorageWrapper.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");


var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", true)
    .AddUserSecrets<Program>()
    .Build();

var services = new ServiceCollection();
services.AddLogging();
services.AddAzureStorageWrapper(config.GetConnectionString("BlobDefaultConnection"));
services.AddScoped<BlobSandbox>();

var serviceProvider = services.BuildServiceProvider();

var blobSandbox = serviceProvider.GetRequiredService<BlobSandbox>();
var result = await blobSandbox.UploadTests();

Console.WriteLine("Uploaded");
Console.WriteLine(string.Join("\n", result));

Console.ReadKey();

public class BlobSandbox
{
    private readonly IBlobWrapperService _blobServiceWrapper;

    public BlobSandbox(IBlobServiceFactory blobServiceFactory)
    {
        _blobServiceWrapper = blobServiceFactory.Create(new BlobContainer("WrapperTest", PublicAccessType.Blob));
    }

    public async Task<string[]> UploadTests()
    {
        var blobImg = new GenericImageBlob("test", "gif", File.OpenRead("Resources\\test.gif"), "AzureStorageWrapper",
            new DefaultImageStrategy(200, 0));
        var urlGif = await _blobServiceWrapper.Upload(blobImg);
        var blobText = new GenericFileBlob("test", "txt", File.OpenRead("Resources\\test.txt"), "AzureStorageWrapper");
        var urlText = await _blobServiceWrapper.Upload(blobText);
        
        return [urlGif, urlText];
    }
}