using System.Text.RegularExpressions;
using Microsoft.Azure.Cosmos;

namespace AzureTestResources.CosmosDb;

public static class TestDbNamingConvention
{
    private const string NameDelimiter = "_";

    public static string GenerateDatabaseId(string namePrefix)
    {
        var utcNow = DateTime.UtcNow;
        var dbName = $"{namePrefix}{NameDelimiter}{utcNow.Ticks}{NameDelimiter}{Guid.NewGuid()}";
        AssertCreationTimeCanBeParsedBackFrom(dbName, utcNow);
        return dbName;
    }


    public static bool IsCreatedEarlierFromNowThan(TimeSpan tolerance, DatabaseProperties d)
    {
        return GetTimeOfCreationOf(d.Id) < DateTime.UtcNow - tolerance;
    }

    public static bool AdheresToNamingConvention(DatabaseProperties properties)
    {
        return Regex.Match(properties.Id, @"^[\w]+-\d+-\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$").Success;
    }

    private static void AssertCreationTimeCanBeParsedBackFrom(string dbName, DateTime utcNow)
    {
        var timeOfCreation = GetTimeOfCreationOf(dbName);
        if (timeOfCreation != utcNow)
        {
            throw new Exception( //bug
              $"Could not get the correct time of creation from {dbName}. " +
              $"Expected {utcNow} but got {timeOfCreation}");
        }
    }

    private static DateTime GetTimeOfCreationOf(string dbName)
    {
        var ticksString = dbName.Split(NameDelimiter)[^2];
        var ticks = long.Parse(ticksString);
        return new DateTime(ticks, DateTimeKind.Utc);
    }
}