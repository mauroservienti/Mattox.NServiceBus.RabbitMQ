using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

namespace Mattox.NServiceBus.RabbitMQ.Tests;

public class RabbitMqEndpointTests
{
    [Fact]
    public void Basic_endpoint_respect_name_and_default_values()
    {
        var expectedEndpointName = "my-endpoint";
        var endpoint = new RabbitMqEndpoint(expectedEndpointName, connectionString: "host=localhost");
        EndpointConfiguration endpointConfiguration = endpoint;
        
        var actualEndpointName = endpointConfiguration.GetSettings().EndpointName();
        
        Assert.Equal(expectedEndpointName, actualEndpointName);
    }
}