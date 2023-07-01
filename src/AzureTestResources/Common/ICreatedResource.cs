namespace AzureTestResources.Common;

public interface ICreatedResource
{
  string Name { get; }
  Task DeleteAsync();
}