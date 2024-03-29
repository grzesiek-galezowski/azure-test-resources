using System.Text.RegularExpressions;

namespace TddXt.AzureTestResources.Common;

public static class TestResourceNamingConvention
{
  private const string NameDelimiter = "-";

  public static string GenerateResourceId(string namePrefix)
  {
    if (namePrefix.Contains(NameDelimiter))
    {
      throw new ArgumentException("cannot contain " + NameDelimiter, nameof(namePrefix));
    }
    var utcNow = DateTime.UtcNow;
    var dbName = $"{namePrefix}{NameDelimiter}{utcNow.Ticks}{NameDelimiter}{Guid.NewGuid():N}";
    AssertCreationTimeCanBeParsedBackFrom(dbName, utcNow);
    return dbName;
  }

  public static bool IsAZombieResource(TimeSpan tolerance, string resourceId)
    => AdheresToNamingConvention(resourceId) && IsCreatedEarlierFromNowThan(tolerance, resourceId);

  public static bool IsCreatedEarlierFromNowThan(TimeSpan tolerance, string resourceId)
  {
    return GetTimeOfCreationOf(resourceId) < DateTime.UtcNow - tolerance;
  }

  public static bool AdheresToNamingConvention(string resourceId)
  {
    return Regex.Match(resourceId,
      $@"^[^{NameDelimiter}]+{NameDelimiter}\d+{NameDelimiter}\w{{8}}\w{{4}}\w{{4}}\w{{4}}\w{{12}}$").Success;
  }

  private static void AssertCreationTimeCanBeParsedBackFrom(string dbName, DateTime originalCreationTime)
  {
    var timeOfCreationParsedBack = GetTimeOfCreationOf(dbName);
    if (timeOfCreationParsedBack != originalCreationTime)
    {
      throw new FormatException(
        $"Could not get the correct time of creation from {dbName}. " +
        $"Expected {originalCreationTime} but got {timeOfCreationParsedBack}");
    }
  }

  private static DateTime GetTimeOfCreationOf(string resourceName)
  {
    var ticksString = resourceName.Split(NameDelimiter)[^2];
    var ticks = long.Parse(ticksString);
    return new DateTime(ticks, DateTimeKind.Utc);
  }
}