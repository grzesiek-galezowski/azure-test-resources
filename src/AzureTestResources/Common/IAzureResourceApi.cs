namespace AzureTestResources.Common;

public interface IAzureResourceApi : IAsyncDisposable
{
  string ConnectionString { get; }
  string Name { get; }
}