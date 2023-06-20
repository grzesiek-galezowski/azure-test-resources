namespace AzureTestResources.AzureStorage.Common;

public interface IAzureResourceApi : IAsyncDisposable
{
  string ConnectionString { get; }
  string Name { get; }
}