using System.Net;
using Azure;

namespace AzureTestResources.Common;

static internal class Assertions
{
  public static void AssertIsHttpCreated<T>(NullableResponse<T> response, string resourceType)
  {
    if (response.GetRawResponse().Status != (int)HttpStatusCode.Created)
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

  public static void AssertNotNull<T>(NullableResponse<T> response, string resourceType, string resourceName)
  {
    if (!response.HasValue)
    {
      throw new InvalidOperationException($"Could not create a {resourceType} {resourceName}. Response is null");
    }
  }
}