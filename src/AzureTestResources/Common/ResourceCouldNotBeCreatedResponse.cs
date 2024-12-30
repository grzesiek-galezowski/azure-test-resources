namespace TddXt.AzureTestResources.Common;

public class ResourceCouldNotBeCreatedResponse<TApi>(Exception rootCause, bool shouldBeRetried)
  : ICreateAzureResourceResponse<TApi>
  where TApi : IAzureResourceApi
{
  public void AssertResourceCreated()
  {
    throw new ResourceCouldNotBeCreatedException(rootCause);
  }

  public bool ShouldBeRetried() => shouldBeRetried;

  public string GetReasonForRetry()
  {
    return rootCause.ToString();
  }

  public TApi CreateResourceApi()
  {
    throw new ResourceCouldNotBeCreatedException(rootCause);
  }
}