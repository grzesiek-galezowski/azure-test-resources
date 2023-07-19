namespace TddXt.AzureTestResources.Common;

public class ResourceCouldNotBeDeletedException : Exception
{
  public ResourceCouldNotBeDeletedException(Exception innerException)
  : base("Resource could not be deleted", innerException)
  {

  }
}