namespace TddXt.AzureTestResources.Common;

public class ResourceCouldNotBeCreatedException : Exception
{
  public ResourceCouldNotBeCreatedException(Exception inner)
    : base("Resource could not be created", inner)
  {
    
  }
}