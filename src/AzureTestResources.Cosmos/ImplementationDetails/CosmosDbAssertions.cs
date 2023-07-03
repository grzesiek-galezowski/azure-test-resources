using System.Net;
using Microsoft.Azure.Cosmos;

namespace AzureTestResources.Cosmos.ImplementationDetails;

internal static class CosmosDbAssertions
{
  public static void AssertIsHttpCreated<T>(Response<T> response, string resourceType)
  {
    if (response.StatusCode != HttpStatusCode.Created)
    {
      throw new InvalidOperationException("Expected the status code to be 201 Created, but " + resourceType + " already exists");
    }
  }

  public static void AssertNamesMatch(string expected, string actual)
  {
    if (expected != actual)
    {
      throw new InvalidOperationException($"Name mismatch, {expected} vs {actual}");
    }
  }
}