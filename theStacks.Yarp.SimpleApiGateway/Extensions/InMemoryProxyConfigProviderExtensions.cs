using Microsoft.Extensions.DependencyInjection;
using theStacks.Yarp.SimpleApiGateway.Utilities;
using Yarp.ReverseProxy.Configuration;
using theStacks.Yarp.SimpleApiGateway.Builders;

namespace theStacks.Yarp.SimpleApiGateway.Extensions;

/// <summary>
/// Extensions used to load current implementation when adding reverse proxy to DI container
/// </summary>
public static class InMemoryProxyConfigProviderExtensions
{
    /// <summary>
    /// Set <see cref="InMemoryProxyConfigProvider"/> as a configuration provider for proxy
    /// </summary>
    /// <param name="builder">Builder used to add reverse proxy to DI container</param>
    /// <param name="routes">List of <see cref="RouteConfig"/></param>
    /// <param name="clusters">List of <see cref="ClusterConfig"/></param>
    /// <returns><see cref="IReverseProxyBuilder"/></returns>
    public static IReverseProxyBuilder LoadFromMemory(this IReverseProxyBuilder builder,
        IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        builder.Services.AddSingleton<IProxyConfigProvider>(new InMemoryProxyConfigProvider(routes, clusters));
        return builder;
    }

    /// <summary>
    /// Set <see cref="InMemoryProxyConfigProvider"/> as a configuration provider for proxy and automatically build cluster configuration.
    /// </summary>
    /// <param name="builder">Builder used to add reverse proxy to DI container</param>
    /// <param name="routes">List of <see cref="RouteConfig"/></param>
    /// <param name="servicesAddresses">Dictionary containing address of given Enum value of <typeparamref name="TServiceType"/></param>
    /// <typeparam name="TServiceType">Enum type used in <see cref="RouteConfigBuilder"/></typeparam>
    /// <returns></returns>
    public static IReverseProxyBuilder LoadFromMemory<TServiceType>(this IReverseProxyBuilder builder,
        IReadOnlyList<RouteConfig> routes, IReadOnlyDictionary<TServiceType, string> servicesAddresses) where TServiceType : Enum
    {
        var clusters = servicesAddresses.Select(service =>
                ClusterUtilities.GetClusterConfig(ClusterUtilities.GetServiceName(service.Key), service.Value))
            .ToList() as IReadOnlyList<ClusterConfig>;
        return LoadFromMemory(builder, routes, clusters);
    }
}