namespace TddXt.AzureTestResources.Common;

public interface ICreateAzureResourceResponse<out TApi> where TApi : IAzureResourceApi
{
  void AssertResourceCreated();
  bool ShouldBeRetried();
  string GetReasonForRetry();
  TApi CreateResourceApi();
}