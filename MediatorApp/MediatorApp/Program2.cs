// using JetBrains.Annotations;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace MediatorApp;
//
// public static class Program
// {
//     public static void Main()
//     {
//         var services = new ServiceCollection();
//         var serviceProvider = services.BuildServiceProvider();
//
//         var descriptor = new HandlerDescriptor(typeof(HelloHandler));
//
//         var factory = ActivatorUtilities.CreateFactory(descriptor.Type, Type.EmptyTypes);
//         var handler = factory(serviceProvider, null);
//
//         var builder = new Builder<HelloRequest, HelloResponse>();
//
//         builder.Use(typeof(LogHandlerFilter<,>));
//     }
// }
//
// public sealed class Builder<TRequest, TResponse>
// {
//     private readonly IServiceProvider _serviceProvider;
//     private readonly List<(ObjectFactory, object[]?)> _list = new();
//
//     public Builder(IServiceProvider serviceProvider)
//     {
//         _serviceProvider = serviceProvider;
//
//         RequestType = typeof(TRequest);
//         ResponseType = typeof(TResponse);
//     }
//
//     public Type RequestType { get; }
//     public Type ResponseType { get; }
//
//     public void Use(Type type, object[]? arguments = null)
//     {
//         var argumentTypes = arguments != null
//             ? Type.GetTypeArray(arguments)
//             : Type.EmptyTypes;
//
//         var factory = ActivatorUtilities.CreateFactory(type, argumentTypes);
//         
//         _list.Add((factory, arguments));
//     }
// }
//
// public abstract class Handler<TRequest, TResponse>
// {
//     public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
// }
//
// public abstract class HandlerFilter<TRequest, TResponse>
// {
//     public delegate Task<TResponse> Next(TRequest request, CancellationToken cancellationToken);
//
//     public abstract Task<TResponse> HandleAsync(TRequest request, Next next, CancellationToken cancellationToken);
// }
//
// public sealed class HandlerDescriptor
// {
//     public HandlerDescriptor(Type type)
//     {
//         Type = type;
//     }
//
//     public Type Type { get; }
// }
//
// public sealed record HelloRequest(string Name);
//
// public sealed record HelloResponse(string Message);
//
// public sealed class HelloHandler : Handler<HelloRequest, HelloResponse>
// {
//     public override Task<HelloResponse> HandleAsync(HelloRequest request, CancellationToken cancellationToken)
//     {
//         var message = $"Hello, {request.Name}!";
//         var response = new HelloResponse(message);
//
//         return Task.FromResult(response);
//     }
// }
//
// public sealed class LogHandlerFilter<TRequest, TResponse> : HandlerFilter<TRequest, TResponse>
// {
//     public override async Task<TResponse> HandleAsync(TRequest request, Next next, CancellationToken cancellationToken)
//     {
//         return await next(request, cancellationToken);
//     }
// }
//
//
// public sealed class CacheHandlerFilter<TRequest, TResponse> : HandlerFilter<TRequest, TResponse>
// {
//     public override async Task<TResponse> HandleAsync(TRequest request, Next next, CancellationToken cancellationToken)
//     {
//         return await next(request, cancellationToken);
//     }
// }
