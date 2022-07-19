// using System.Buffers;
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
//         IHandlerActivator handlerActivator = new HandlerActivator<TestHandler>();
//         IHandlerFilterActivator[] handlerFilterActivators =
//         {
//             new HandlerFilterActivator<HandlerFilterA>(),
//             new HandlerFilterActivator<HandlerFilterB>(),
//             new HandlerFilterActivator<HandlerFilterC>(),
//         };
//
//         HandlerFilterBehaviour[] handlerFilterBehaviours =
//         {
//             HandlerFilterBehaviour.Default,
//             HandlerFilterBehaviour.Lazy,
//             HandlerFilterBehaviour.LazyPerCall,
//         };
//
//         var handler = new HandlerPipeline(
//             serviceProvider, 
//             handlerActivator, 
//             handlerFilterActivators,
//             handlerFilterBehaviours);
//
//         handler.Handle("Hello, World!");
//     }
// }
//
// public sealed class HandlerPipeline : Handler
// {
//     private readonly IServiceProvider _serviceProvider;
//     private readonly IHandlerActivator _handlerActivator;
//     private readonly IHandlerFilterActivator[] _handlerFilterActivators;
//     private readonly HandlerFilterBehaviour[] _handlerFilterBehaviours;
//     private readonly Func<string, string> _next;
//     
//     public HandlerPipeline(IServiceProvider serviceProvider,
//         IHandlerActivator handlerActivator,
//         IHandlerFilterActivator[] handlerFilterActivators, 
//         HandlerFilterBehaviour[] handlerFilterBehaviours)
//     {
//         _serviceProvider = serviceProvider;
//         _handlerActivator = handlerActivator;
//         _handlerFilterActivators = handlerFilterActivators;
//         _handlerFilterBehaviours = handlerFilterBehaviours;
//         _next = Next;
//     }
//
//     public override string Handle(string request)
//     {
//         if (_handlerFilterActivators.Length == 0)
//         {
//             var handler = _handlerActivator.CreateHandler(_serviceProvider);
//             return handler.Handle(request);
//         }
//
//         _handlerFilters = ArrayPool<HandlerFilter>.Shared.Rent(_handlerFilterActivators.Length);
//         
//         try
//         {
//             return Next(request);
//         }
//         finally
//         {
//             ArrayPool<HandlerFilter>.Shared.Return(_handlerFilters);
//         }
//     }
//
//     private int _nextCallFilterIndex = 0;
//     private int _nextCreateFilterIndex = 0;
//
//     private HandlerFilter[] _handlerFilters = null!;
//     private Handler? _handler;
//     
//     private string Next(string request)
//     {
//         if (_nextCallFilterIndex == _handlerFilterActivators.Length)
//         {
//             if (_handler == null)
//             {
//                 _handler = _handlerActivator.CreateHandler(_serviceProvider);
//             }
//
//             return _handler.Handle(request);
//         }
//
//         if (_nextCallFilterIndex == _nextCreateFilterIndex)
//         {
//             while (_nextCreateFilterIndex < _handlerFilterActivators.Length &&
//                    _nextCreateFilterIndex - _nextCallFilterIndex < 3)
//             {
//                 var activator = _handlerFilterActivators[_nextCreateFilterIndex];
//                 _handlerFilters[_nextCreateFilterIndex] = activator.CreateHandlerFilter(_serviceProvider);
//
//                 _nextCreateFilterIndex++;
//             }
//
//             if (_nextCreateFilterIndex == _handlerFilterActivators.Length)
//             {
//                 _handler = _handlerActivator.CreateHandler(_serviceProvider);
//             }
//         }
//
//         var filter = _handlerFilters[_nextCallFilterIndex];
//
//         _nextCallFilterIndex++;
//
//         return filter.Handle(request, _next);
//     }
// }
//
// public abstract class Handler
// {
//     public abstract string Handle(string request);
// }
//
// public abstract class HandlerFilter
// {
//     public abstract string Handle(string request, Func<string, string> next);
// }
//
// public enum HandlerFilterBehaviour
// {
//     Default,
//     Lazy,
//     LazyPerCall,
// }
//
// public interface IHandlerActivator
// {
//     public Handler CreateHandler(IServiceProvider serviceProvider);
// }
//
// public interface IHandlerFilterActivator
// {
//     public HandlerFilter CreateHandlerFilter(IServiceProvider serviceProvider);
// }
//
// public sealed class HandlerActivator<THandler> : IHandlerActivator
//     where THandler : Handler
// {
//     public Handler CreateHandler(IServiceProvider serviceProvider)
//     {
//         return ActivatorUtilities.CreateInstance<THandler>(serviceProvider);
//     }
// }
//
// public sealed class HandlerFilterActivator<THandlerFilter> : IHandlerFilterActivator
//     where THandlerFilter : HandlerFilter
// {
//     public HandlerFilter CreateHandlerFilter(IServiceProvider serviceProvider)
//     {
//         return ActivatorUtilities.CreateInstance<THandlerFilter>(serviceProvider);
//     }
// }
//
// public class TestHandler : Handler
// {
//     public TestHandler()
//     {
//         Console.WriteLine($"{Name} Created!");
//     }
//
//     public string Name => GetType().Name;
//
//     public override string Handle(string request)
//     {
//         Console.WriteLine($"{Name}: " + request);
//
//         return request;
//     }
// }
//
// public class HandlerFilterBase : HandlerFilter
// {
//     public HandlerFilterBase()
//     {
//         Console.WriteLine($"{Name} Created!");
//     }
//
//     public string Name => GetType().Name;
//
//     public override string Handle(string request, Func<string, string> next)
//     {
//         var type = GetType();
//
//         Console.WriteLine($"{Name} Request: " + request);
//
//         var response = next(request);
//         Console.WriteLine($"{Name} Response: " + request);
//
//         return response;
//     }
// }
//
// public class HandlerFilterA : HandlerFilterBase
// {
// }
//
// public class HandlerFilterB : HandlerFilterBase
// {
// }
//
// public class HandlerFilterC : HandlerFilterBase
// {
// }