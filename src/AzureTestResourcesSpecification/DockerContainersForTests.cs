using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Extensions.Logging.NUnit;
using TddXt.AzureTestResources.Data.Tables;
using Testcontainers.Azurite;
using Testcontainers.CosmosDb;
using Testcontainers.ServiceBus;

namespace TddXt.AzureTestResourcesSpecification;

public static class DockerContainersForTests
{
  public static async Task<ServiceBusContainer> StartServiceBusContainer()
  {
    var container = new ServiceBusBuilder()
      .WithLogger(new NUnitLogger("lol")) //bug 
      .WithAcceptLicenseAgreement(true).Build();
    await container.StartAsync();
    await TestContext.Progress.WriteLineAsync(container.GetConnectionString());
    return container;
  }

  public static async Task<AzuriteContainer> StartAzuriteContainer()
  {
    var container = new AzuriteBuilder()
      .WithLogger(new NUnitLogger("lol")) //bug 
      .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
      .Build();
    await container.StartAsync();
    await TestContext.Progress.WriteLineAsync(container.GetConnectionString());
    return container;
  }

  public static async Task<CosmosDbContainer> StartCosmosDbContainer()
  {
    var container = new CosmosDbBuilder()
      .WithLogger(new NUnitLogger("lol")) //bug 
      // not supported by linux version: 
      // .WithEnvironment("EnableTableEndpoint", "true")
      // .WithPortBinding(CosmosTestTableConfig.DefaultPortNumber, assignRandomHostPort: true)
      .Build();
    await container.StartAsync();
    await TestContext.Progress.WriteLineAsync(container.GetConnectionString());
    return container;
  }

  public static async Task<IContainer> StartCosmosDbContainer2()
  {
    var container = new ContainerBuilder()
        .WithLogger(new NUnitLogger("lol")) //bug
        .WithImage("ghcr.io/pikami/cosmium")
        .WithPortBinding(8081, 8081) //bug cleanup!!!
        //.WithEnvironment("COSMIUM_DISABLEAUTH", "true")
        //.WithEnvironment("COSMIUM_PERSIST", "/save.json")
        //.WithBindMount("./save.json", "/save.json")
        //.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8081))
        .Build();

    await container.StartAsync();
    return container;
  }
}