namespace TddXt.AzureTestResources.Common;

public class ResourceCouldNotBeDeletedException(Exception innerException)
  : Exception("Resource could not be deleted", innerException);