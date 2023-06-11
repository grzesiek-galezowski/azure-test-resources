using System.Net;
using Azure.Data.Tables;
using AzureTestResources.CosmosDbNoSqlApi;
using Microsoft.Azure.Cosmos;

namespace AzureTestResources.CosmosDbTableApi;

public static class ZombieTableCleanup //bug use in tests
{
    private static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(2);

    public static async Task DeleteZombieTables()
    {
        await DeleteZombieTables(DefaultTolerance);
    }

    private static async Task DeleteZombieTables(TimeSpan tolerance)
    {
        var client = new TableServiceClient(
          CosmosTestDatabaseConfig.Default().ConnectionString); //bug works only with an emulator

        foreach (var table in client.Query()
                   .Where(table => TestResourceNamingConvention.AdheresToNamingConvention(table.Name))
                   .Where(table => TestResourceNamingConvention.IsCreatedEarlierFromNowThan(
                     tolerance, table.Name)))
        {
          //bug catch exception when table not found
          await client.GetTableClient(table.Name).DeleteAsync();
        }
    }
}