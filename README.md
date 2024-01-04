<img src="assets/icon.png" width="100" />

# NServiceBoXes.Endpoints.RabbitMQ

NServiceBoXes Endpoints simplify [NServiceBus endpoints](https://docs.particular.net/nservicebus/) configuration by providing for supported transports a corresponding NServiceBoXes endpoint with sensible defaults. `NServiceBoXes.Endpoints.RabbitMQ` is the NServiceBoXes endpoint for the [NServiceBus RabbitMQ transport](https://docs.particular.net/transports/rabbitmq/).

Creating and starting a RabbitMQ endpoint is as easy as:

```csharp
var endpoint = new RabbitMqEndpoint("my-endpoint", connectionString: "host=localhost");
var endpointInstance = await endpoint.Start();
```

## Microsoft configuration extension support

NServiceBoXes endpoints can be configured through the [`Microsoft.Extensions.Configuration`](https://www.nuget.org/packages/Microsoft.Extensions.Configuration). The above-presented RabbitMQ endpoint can be configured as follows:

```csharp
Host.CreateDefaultBuilder()
    .UseNServiceBus(hostBuilderContext => new RabbitMqEndpoint(hostBuilderContext.Configuration))
    .Build();
```

The endpoint will retrieve values from the `IConfiguration` object instance.

## Supported endpoints

For more information on all the supported endpoints, refer to the [Mattox.NServiceBus](https://github.com/mauroservienti/Mattox.NServiceBus#supported-endpoints) repository.

## How to get it

- Pre-releases are available on [Feedz.io](https://feedz.io/) ([public feed](https://f.feedz.io/mauroservienti/pre-releases/nuget/index.json))
- Releases on [NuGet.org](https://www.nuget.org/packages?q=NServiceBoXes)







---

Icon — [Box by Angriawan Ditya Zulkarnain](https://thenounproject.com/icon/box-1298424/) from [Noun Project](https://thenounproject.com/browse/icons/term/box/) (CC BY 3.0)
