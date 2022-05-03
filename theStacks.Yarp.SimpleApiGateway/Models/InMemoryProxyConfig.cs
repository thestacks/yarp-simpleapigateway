using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace theStacks.Yarp.SimpleApiGateway.Models;

/// <summary>
/// Class which holds Proxy Configuration
/// </summary>
internal class InMemoryProxyConfig : IProxyConfig
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    /// <summary>
    /// List of registered routes
    /// </summary>
    public IReadOnlyList<RouteConfig> Routes { get; }
    /// <summary>
    /// List of registered clusters
    /// </summary>
    public IReadOnlyList<ClusterConfig> Clusters { get; }
    /// <summary>
    /// ChangeToken responsible for dispatching notification about change
    /// </summary>
    public IChangeToken ChangeToken { get; }

    public InMemoryProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        Routes = routes;
        Clusters = clusters;
        ChangeToken = new CancellationChangeToken(_cancellationTokenSource.Token);
    }

    /// <summary>
    /// Method used to signal change of the configuration
    /// </summary>
    internal void SignalChange()
    {
        _cancellationTokenSource.Cancel();
    }
}