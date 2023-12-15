using Microsoft.Extensions.Configuration;
using NServiceBus;

namespace Mattox.NServiceBus.RabbitMQ;

public class RabbitMqEndpoint : NServiceBusEndpoint<RabbitMQTransport>
{
    private readonly string? _userDefinedConnectionString;
    private readonly RoutingTopology? _userDefinedRoutingTopology;

    public RabbitMqEndpoint(IConfiguration configuration)
        : base(configuration)
    {
        
    }

    public RabbitMqEndpoint(string endpointName, RoutingTopology? routingTopology = null, string? connectionString = null, IConfiguration? configuration = null)
        : base(endpointName, configuration)
    {
        _userDefinedConnectionString = connectionString;
        _userDefinedRoutingTopology = routingTopology;
    }

    protected override RabbitMQTransport CreateTransport(IConfigurationSection? transportConfigurationSection)
    {
        var routingTopology = _userDefinedRoutingTopology ?? GetRoutingTopologyFromConfigurationOrDefault(transportConfigurationSection);
        var connectionString = _userDefinedConnectionString ?? GetConnectionStringFromConfigurationOrThrow(transportConfigurationSection);
        
        var transport = new RabbitMQTransport(
            routingTopology, 
            connectionString);

        ApplyCommonTransportSettings(transportConfigurationSection, transport);
        
        // TODO: RabbitMQ transport customizations from configuration section
        
        return transport;
    }

    static RoutingTopology GetRoutingTopologyFromConfigurationOrDefault(IConfigurationSection? transportConfigurationSection)
    {
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

    static string GetConnectionStringFromConfigurationOrThrow(IConfigurationSection? transportConfigurationSection)
    {
        const string connectionStringConfigurationSectionValue = "NServiceBus:EndpointConfiguration:Transport:ConnectionString";
        if (transportConfigurationSection == null)
        {
            throw new ArgumentException( 
            "The endpoint requires a connection string. Either pass it as a constructor parameter or set the " +
            $"it in the configuration using the {connectionStringConfigurationSectionValue} configuration section value.");
        }

        return transportConfigurationSection["ConnectionString"]
               ?? throw new ArgumentException(
                   "ConnectionString cannot be null. Make sure the " +
                   $"{connectionStringConfigurationSectionValue} configuration section is set.");
    }
}