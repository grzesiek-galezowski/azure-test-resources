namespace TddXt.AzureTestResources.Common;

public interface ICreatedResourcesPool
{
  Task<IEnumerable<ICreatedResource>> LoadResources();
}