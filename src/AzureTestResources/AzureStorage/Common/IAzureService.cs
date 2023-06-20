namespace AzureTestResources.AzureStorage.Common;

public interface IAzureService<TApi> where TApi : IAzureResourceApi
{
    Task<ICreateAzureResourceResponse<TApi>> CreateResourceInstance();
}