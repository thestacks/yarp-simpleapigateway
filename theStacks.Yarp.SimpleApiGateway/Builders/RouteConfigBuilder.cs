using theStacks.Yarp.SimpleApiGateway.Models;
using theStacks.Yarp.SimpleApiGateway.Utilities;
using Yarp.ReverseProxy.Configuration;

namespace theStacks.Yarp.SimpleApiGateway.Builders;

/// <summary>
/// Builder class for <see cref="RouteConfig"/> to quickly build route.
/// </summary>
public class RouteConfigBuilder
{
    private string? _id;
    private string? _clusterId;
    private RouteMatch? _routeMatch;
    private int? _order;
    
    public static readonly RouteConfigBuilder Builder = new();

    /// <summary>
    /// Set identifier of the route. This should be unique across whole config.
    /// </summary>
    /// <param name="id">Identifier of the route</param>
    public RouteConfigBuilder WithId(string? id)
    {
        _id = id;
        return this;
    }

    /// <summary>
    /// Set destination cluster.
    /// </summary>
    /// <param name="clusterId">Identifier of the destination cluster.</param>
    public RouteConfigBuilder ToCluster(string clusterId)
    {
        _clusterId = clusterId;
        return this;
    }

    /// <summary>
    /// Rules for the route.
    /// </summary>
    /// <param name="match"><see cref="RouteMatch"/> containing rules of the route</param>
    public RouteConfigBuilder WithRouteMatch(RouteMatch match)
    {
        _routeMatch = match;
        return this;
    }

    /// <summary>
    /// Simple rule path match for the route.
    /// </summary>
    /// <param name="path">Path of the rule.</param>
    /// <param name="catchAll">If any character occuring after given <paramref name="path"/> should be also redirected, set it to true. By default: false.</param>
    /// <returns></returns>
    public RouteConfigBuilder WithPathMatch(string path, bool catchAll = false)
    {
        var routePath = path;
        if (catchAll)
        {
            if (!routePath.EndsWith("/"))
                routePath += "/";
            routePath += "{**catchall}";
        }

        return WithRouteMatch(new RouteMatch {Path = routePath});
    }

    /// <summary>
    /// Order of the route.
    /// </summary>
    /// <param name="order">Order of the route.</param>
    /// <returns></returns>
    public RouteConfigBuilder HasOrder(int? order)
    {
        _order = order;
        return this;
    }

    /// <summary>
    /// Enum type, which maps value into cluster name.
    /// </summary>
    /// <param name="service">Enum value of the service.</param>
    /// <typeparam name="TServiceType">Enum type containing list of services</typeparam>
    public RouteConfigBuilder ToService<TServiceType>(TServiceType service) where TServiceType : Enum =>
        ToCluster(ClusterUtilities.GetServiceName(service));

    /// <summary>
    /// Finalization method, should be used to combine given values.
    /// </summary>
    /// <returns></returns>
    public RouteConfig Build() => new()
    {
        RouteId = !string.IsNullOrEmpty(_id) ? _id : Guid.NewGuid().ToString(),
        ClusterId = _clusterId ?? throw new ArgumentNullException("ClusterId is missing"),
        Order = _order,
        Match = _routeMatch ?? throw new ArgumentNullException("RouteMatch is missing")
    };
}