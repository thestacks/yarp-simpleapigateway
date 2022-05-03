using Yarp.ReverseProxy.Configuration;

namespace theStacks.Yarp.SimpleApiGateway.Utilities;

internal static class ClusterUtilities
{
    private const string DefaultDestinationName = "primary";
    public static string GetServiceName<TServiceType>(TServiceType serviceType) where TServiceType : Enum =>
        serviceType.ToString();

    /// <summary>
    /// Get the cluster configuration containing single destination.
    /// </summary>
    /// <param name="name">Name of the cluster used as ClusterId</param>
    /// <param name="address">Destination address</param>
    /// <returns></returns>
    public static ClusterConfig GetClusterConfig(string name, string address) => new()
    {
        ClusterId = name,
        Destinations = new Dictionary<string, DestinationConfig>
        {
            {
                DefaultDestinationName, new DestinationConfig
                {
                    Address = address
                }
            }
        }
    };
}