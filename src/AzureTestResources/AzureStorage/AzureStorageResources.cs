using System.Net;
using Azure;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;

namespace AzureTestResources.AzureStorage;

public static class AzureStorageResources
{
  public static async Task<StorageTestQueue> CreateQueue(ILogger logger, CancellationToken cancellationToken)
  {
    return await CreateQueue(
      "DefaultEndpointsProtocol=https;" +
      "AccountName=devstoreaccount1;" +
      "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
      "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;", 
      logger, 
      cancellationToken);
  }

  private static async Task<StorageTestQueue> CreateQueue(
    string connectionString,
    ILogger logger,
    CancellationToken ct)
  {

    var client = new QueueServiceClient(
      connectionString
    );

    var response = await AzureStorageRequestPolicyFactory.CreateCreateResourcePolicy(logger)
      .ExecuteAsync(async () =>
      {
        var resourceId = TestResourceNamingConvention.GenerateResourceId(
          "q" /* see https://learn.microsoft.com/en-us/rest/api/storageservices/naming-queues-and-metadata#queue-names */);
        var response = await client.CreateQueueAsync(resourceId, cancellationToken: ct);
        AssertValidResponse(response, resourceId);
        return response;
      });
    return new StorageTestQueue(client, response.Value.Name, ct, connectionString);
  }

  private static void AssertValidResponse(Response<QueueClient> response, string resourceName)
  {
    if (!response.HasValue)
    {
      throw new InvalidOperationException("Could not create a subscription " + resourceName);
    }

    if (resourceName != response.Value.Name)
    {
      throw new InvalidOperationException("Naming mismatch");
    }

    if(response.GetRawResponse().Status != (int)HttpStatusCode.Created)
    {
      throw new InvalidOperationException("Queue already exists");
    }
  }
}//bug zombie cleanup