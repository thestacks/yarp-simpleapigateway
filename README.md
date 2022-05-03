# About

This library contains simple implementation of the [YARP](https://github.com/microsoft/reverse-proxy) configuration provider persisted in memory.
It should be used for simple API Gateway scenarios, usually when designing solutions written in BFF pattern.

# Sample usage

```
enum ServiceType
{
    HELLO,
    USERS
}

var routes = new List<RouteConfig>
{
    RouteConfigBuilder.Builder.WithId("users-all").ToService(ServiceType.USERS).WithPathMatch("/api/users", true).Build(),
    RouteConfigBuilder.Builder.WithId("hello").ToService(ServiceType.HELLO).WithPathMatch("/api/hello").Build()
} as IReadOnlyList<RouteConfig>;
var servicesUrls = new Dictionary<ServiceType, string> { 
    { ServiceType.HELLO, "https://localhost:10001" },
    { ServiceType.USERS, "https://localhost:20000" }
};
services.AddReverseProxy()
    .LoadFromMemory(routes, servicesUrls);
```
Above code will add in memory configuration provider and proxy would:
- redirect everything that matches /api/users (for example `/api/users`, `/api/users/some/path` to `localhost:20000/{REQUEST_PATH}`)
- redirect exactly `/api/hello` to `localhost:100001/api/hello`

# Issues

If you have any issues, problems, questions, feel free to open an issue on GitHub.
