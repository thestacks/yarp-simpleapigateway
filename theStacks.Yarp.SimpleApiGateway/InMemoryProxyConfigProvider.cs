using theStacks.Yarp.SimpleApiGateway.Models;
using Yarp.ReverseProxy.Configuration;

namespace theStacks.Yarp.SimpleApiGateway;

/// <summary>
/// <see cref="IProxyConfig"/> provider which holds the configuration in volatile memory.
/// </summary>
public class InMemoryProxyConfigProvider : IProxyConfigProvider
{
    private volatile InMemoryProxyConfig _config;
    
    public InMemoryProxyConfigProvider(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        _config = new InMemoryProxyConfig(routes, clusters);
    }

    /// <summary>
    /// Get current configuration of the proxy.
    /// </summary>
    /// <returns></returns>
    public IProxyConfig GetConfig() => _config;

    /// <summary>
    /// Signal change of the configuration and reload it.
    /// </summary>
    /// <param name="routes">List of new routes</param>
    /// <param name="clusters">List of new clusters</param>
    public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        var oldConfig = _config;
        _config = new InMemoryProxyConfig(routes, clusters);
        oldConfig.SignalChange();
    }
}