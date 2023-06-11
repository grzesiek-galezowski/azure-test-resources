using System.Security.Policy;
using AzureTestResources.CosmosDbTableApi;
using Extensions.Logging.NUnit;

namespace AzureTestResourcesSpecification;

internal class CosmosTableApiSpecification
{
  [TestCase(1)]
  [TestCase(2)]
  [TestCase(3)]
  [TestCase(4)]
  [TestCase(5)]
  [TestCase(6)]
  [TestCase(7)]
  [TestCase(8)]
  [TestCase(9)]
  [TestCase(10)]
  [TestCase(11)]
  [TestCase(12)]
  [TestCase(13)]
  [TestCase(14)]
  [TestCase(15)]
  [TestCase(16)]
  [TestCase(21)]
  [TestCase(22)]
  [TestCase(23)]
  [TestCase(24)]
  [TestCase(25)]
  [TestCase(26)]
  [TestCase(27)]
  [TestCase(28)]
  [TestCase(29)]
  [TestCase(30)]
  [TestCase(31)]
  [TestCase(32)]
  [TestCase(33)]
  [TestCase(34)]
  [TestCase(35)]
  [TestCase(36)]
  public async Task ShouldWHAT(int testNo)
  {
    //bug retry on generating the same name
    //bug too many resources?
    //bug cleanup zombie

    //GIVEN
    await using var config = await CosmosTestTable.Create(new NUnitLogger("table"));
    
    //WHEN

    //THEN
  }
}