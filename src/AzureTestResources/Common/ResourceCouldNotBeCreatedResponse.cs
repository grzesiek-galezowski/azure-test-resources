namespace AzureTestResources.Common;

public class ResourceCouldNotBeCreatedResponse<TApi> : ICreateAzureResourceResponse<TApi>
  where TApi : IAzureResourceApi
{
  private readonly Exception _rootCause;
  private readonly bool _shouldBeRetried;

  public ResourceCouldNotBeCreatedResponse(Exception rootCause, bool shouldBeRetried)
  {
    _rootCause = rootCause;
    _shouldBeRetried = shouldBeRetried;
  }

  public void AssertResourceCreated()
  {
    throw _rootCause; //bug wrap
  }

  public bool ShouldBeRetried() => _shouldBeRetried;

  public string GetReasonForRetry()
  {
    return _rootCause.ToString();
  }

  public TApi CreateResourceApi()
  {
    throw _rootCause; //bug wrap
  }
}