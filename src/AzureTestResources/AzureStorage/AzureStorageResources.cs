using Azure;
using Azure.Storage.Queues;

namespace AzureTestResources.AzureStorage;

public static class AzureStorageResources
{
  public static async Task<StorageTestQueue> CreateQueue(CancellationToken cancellationToken)
  {
    return await CreateQueue(
      "DefaultEndpointsProtocol=https;" +
      "AccountName=devstoreaccount1;" +
      "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
      "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;", cancellationToken);
  }

  private static async Task<StorageTestQueue> CreateQueue(string connectionString, CancellationToken ct)
  {
    var resourceId = TestResourceNamingConvention.GenerateResourceId(
      "q" /* see https://learn.microsoft.com/en-us/rest/api/storageservices/naming-queues-and-metadata#queue-names */);
    
    var client = new QueueClient(
      connectionString,
      resourceId
    );

    var response = await client.CreateAsync(cancellationToken: ct);
    AssertValidResponse(response, resourceId);

    return new StorageTestQueue(client, resourceId, ct, connectionString);
  }

  private static void AssertValidResponse(Response response, string resourceName)
  {
    if (response == null || response.Status != 201)
    {
      throw new InvalidOperationException("Could not create a queue " + resourceName);
    }
  }
}