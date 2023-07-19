namespace TddXt.AzureTestResources.Common;

public interface IAzureService<TApi> where TApi : IAzureResourceApi
{
  Task<ICreateAzureResourceResponse<TApi>> CreateResourceInstance();
}