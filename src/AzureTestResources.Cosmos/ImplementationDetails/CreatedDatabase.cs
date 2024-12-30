using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using TddXt.AzureTestResources.Common;

namespace TddXt.AzureTestResources.Cosmos.ImplementationDetails;

public class CreatedDatabase(DatabaseProperties databaseProperties, CosmosClient cosmosClient, ILogger logger)
  : ICreatedResource
{
  public string Name => databaseProperties.Id;
  public async Task DeleteAsync()
  {
    try
    {
      var database = cosmosClient.GetDatabase(databaseProperties.Id);
      await DeleteContainers(database, cosmosClient);
      await database.DeleteAsync();
    }
    catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
    {
      throw new ResourceCouldNotBeDeletedException(e);
    }
  }

  /// <summary>
  /// The whole point of deleting the containers first is that the cosmos db emulator
  /// has a limit on container count, so we're trying to trim this as soon as possible.
  /// </summary>
  private async Task DeleteContainers(Database database, CosmosClient client)
  {
    var containerPropertiesList = await DatabaseInformation.GetContainerList(database);
    logger.LogInformation($"Database has {containerPropertiesList.Count} containers");
    foreach (var containerProperties in containerPropertiesList)
    {
      try
      {
        logger.LogInformation($"Deleting container {containerProperties.Id}");
        var container = client.GetContainer(database.Id, containerProperties.Id);
        await container.DeleteContainerAsync();
        logger.LogInformation($"Deleted container {containerProperties.Id}");
      }
      catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
      {
        logger.LogWarning($"Container {containerProperties.Id} already deleted");
      }
    }
  }
}