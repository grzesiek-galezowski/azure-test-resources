using System.Text;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using AzureTestResources.AzureStorage;
using Extensions.Logging.NUnit;

namespace AzureTestResourcesSpecification;

public class AzureStorageBlobContainersSpecification
{
  [TestCase(1)]
  [TestCase(2)]
  [TestCase(3)]
  [TestCase(4)]
  [TestCase(5)]
  [TestCase(6)]
  [TestCase(7)]
  [TestCase(8)]
  [TestCase(9)]
  [TestCase(10)]
  [TestCase(11)]
  [TestCase(12)]
  [TestCase(13)]
  [TestCase(14)]
  [TestCase(15)]
  [TestCase(16)]
  [TestCase(21)]
  [TestCase(22)]
  [TestCase(23)]
  [TestCase(24)]
  [TestCase(25)]
  [TestCase(26)]
  [TestCase(27)]
  [TestCase(28)]
  [TestCase(29)]
  [TestCase(30)]
  [TestCase(31)]
  [TestCase(32)]
  [TestCase(33)]
  [TestCase(34)]
  [TestCase(35)]
  [TestCase(36)]
  public async Task ShouldCreateAzureStorageBlobContainer(int testNo)
  {
    var messageText = "lol";
    await using var container = await AzureStorageResources.CreateBlobContainer(
      new NUnitLogger("blob"), new CancellationToken());
    var containerClient = new BlobContainerClient(container.ConnectionString, container.Name);

    var blobClient = containerClient.GetBlobClient("myBlob");
    await Upload(messageText, blobClient);

    var response = await blobClient.DownloadAsync();

    var responseText = await GetStringFrom(response);
    Assert.AreEqual(messageText, responseText);
    //bug zombie removal
  }

  private static async Task Upload(string messageText, BlobClient blobClient)
  {
    byte[] byteArray = Encoding.ASCII.GetBytes(messageText);
    using MemoryStream stream = new MemoryStream(byteArray);
    await blobClient.UploadAsync(stream, true);
  }

  private static async Task<string> GetStringFrom(Response<BlobDownloadInfo> response)
  {
    using StreamReader reader = new StreamReader(response.Value.Content);
    var text = await reader.ReadToEndAsync();
    return text;
  }
}