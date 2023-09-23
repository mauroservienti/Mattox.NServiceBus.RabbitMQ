using Microsoft.Extensions.Configuration;
using NServiceBus;

namespace NServiceBoXes.Endpoints.RabbitMQ;

public class RabbitMqEndpoint : NServiceBusEndpoint<RabbitMQTransport>
{
    const string transportConfigurationSectionKey = "NServiceBus:EndpointConfiguration:Transport";
    readonly RoutingTopology _routingTopology;
    readonly string _connectionString;

    public RabbitMqEndpoint(IConfiguration configuration)
        : base(GetEndpointNameFromConfigurationOrThrow(configuration), configuration)
    {
        _routingTopology = GetRoutingTopologyFromConfigurationOrDefault(configuration);
        _connectionString = GetConnectionStringFromConfigurationOrThrow(configuration);
    }

    public RabbitMqEndpoint(string endpointName, RoutingTopology? routingTopology = null, string? connectionString = null, IConfiguration? configuration = null)
        : base(endpointName, configuration)
    {
        _connectionString = connectionString ?? GetConnectionStringFromConfigurationOrThrow(configuration);
        _routingTopology = routingTopology ?? GetRoutingTopologyFromConfigurationOrDefault(configuration);
    }

    protected override RabbitMQTransport CreateTransport(IConfigurationSection? endpointConfigurationSection)
    {
        var transport = new RabbitMQTransport(
            _routingTopology, 
            _connectionString);

        if (endpointConfigurationSection?.GetSection("Transport") is { } transportConfigurationSection)
        {
            // TODO: RabbitMQ transport customizations from configuration section
        }
        
        return transport;
    }

    static RoutingTopology GetRoutingTopologyFromConfigurationOrDefault(IConfiguration? configuration)
    {
        var transportConfigurationSection = configuration?.GetSection(transportConfigurationSectionKey);
        
        var queueType = transportConfigurationSection?["QueueType"] switch
        {
            "Classic" => QueueType.Classic,
            "Quorum" => QueueType.Quorum,
            _ => QueueType.Quorum //default value
        };
        
        var routingTopology = transportConfigurationSection?["RoutingTopology"] switch
        {
            "Conventional" => RoutingTopology.Conventional(queueType),
            "Direct" => RoutingTopology.Direct(queueType),
            _ => RoutingTopology.Conventional(queueType) //default value
        };
        
        return routingTopology;
    }

    static string GetConnectionStringFromConfigurationOrThrow(IConfiguration? configuration)
    {
        const string connectionStringConfigurationSectionValue = "NServiceBus:EndpointConfiguration:Transport:ConnectionString";
        if (configuration == null)
        {
            throw new ArgumentNullException(
            nameof(configuration), 
            "The endpoint requires a connection string. Either pass it as a constructor parameter or set the " +
            $"it in the configuration using the {connectionStringConfigurationSectionValue} configuration section value.");
        }

        return configuration.GetSection(transportConfigurationSectionKey)["ConnectionString"]
               ?? throw new ArgumentException(
                   "ConnectionString cannot be null. Make sure the " +
                   $"{connectionStringConfigurationSectionValue} configuration section is set.");
    }
}