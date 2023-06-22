namespace AzureTestResources.Common;

public interface ICreateAzureResourceResponse<out TApi> where TApi : IAzureResourceApi
{
  void AssertValidResponse();
  bool ShouldBeRetried();
  string GetReasonForRetry();
  TApi CreateResourceApi();
}