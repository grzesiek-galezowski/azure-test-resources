using System.Net;
using Microsoft.Azure.Cosmos;

namespace AzureTestResources.CosmosDbNoSqlApi;

public static class ZombieDatabaseCleanup
{
    private static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(2);

    public static async Task DeleteZombieDatabases()
    {
        await DeleteZombieDatabases(DefaultTolerance);
    }

    private static async Task DeleteZombieDatabases(TimeSpan tolerance)
    {
        using var client = CosmosClientFactory.CreateCosmosClient(
          CosmosTestDatabaseConfig.Default());

        var databaseList = await DatabaseInformation.GetDatabaseList(client);


        foreach (var databaseProperties in databaseList
                   .Where(properties => TestResourceNamingConvention.AdheresToNamingConvention(properties.Id))
                   .Where(properties => TestResourceNamingConvention.IsCreatedEarlierFromNowThan(
                     tolerance, properties.Id)))
        {
            try
            {
                //bug make this more efficient?
                var database = client.GetDatabase(databaseProperties.Id);
                await database.DeleteAsync();
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                //silently ignore these databases
            }
        }
    }
}