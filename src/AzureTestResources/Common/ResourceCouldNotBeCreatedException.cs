namespace TddXt.AzureTestResources.Common;

public class ResourceCouldNotBeCreatedException(Exception inner) : Exception("Resource could not be created", inner);