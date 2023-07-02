namespace AzureTestResources.Common;

public class ResourceCouldNotBeCreateException : Exception
{
  public ResourceCouldNotBeCreateException(Exception inner)
    : base("Resource could not be created", inner)
  {
    
  }
}