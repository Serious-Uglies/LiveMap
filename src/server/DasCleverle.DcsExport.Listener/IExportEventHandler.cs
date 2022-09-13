using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Listener;

public interface IExportEventHandler
{
    Task HandleEventAsync(IExportEvent exportEvent, CancellationToken token);
}

public interface IExportEventHandler<T>
{
    Task HandleEventAsync(IExportEvent<T> exportEvent, CancellationToken token);
}

internal class GenericExportEventHandlerPropgator : IExportEventHandler
{
    private delegate Task GenericHandlerDelegate(object handlers, IExportEvent exportEvent, CancellationToken token);

    private static readonly ConcurrentDictionary<Type, Type?> EventHandlerTypeMap = new ConcurrentDictionary<Type, Type?>();
    private static readonly ConcurrentDictionary<Type, GenericHandlerDelegate?> HandlerMethodMap = new ConcurrentDictionary<Type, GenericHandlerDelegate?>();

    private static readonly MethodInfo HandleGenericEventMethod = typeof(GenericExportEventHandlerPropgator)
        .GetMethod(nameof(HandleGenericEventAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

    private readonly IServiceProvider _services;

    public GenericExportEventHandlerPropgator(IServiceProvider services)
    {
        _services = services;
    }

    public async Task HandleEventAsync(IExportEvent exportEvent, CancellationToken token)
    {
        if (exportEvent == null)
        {
            throw new ArgumentNullException(nameof(exportEvent));
        }

        var handlerType = GetHandlerType(exportEvent);

        if (handlerType == null)
        {
            return;
        }

        var handlers = _services.GetService(handlerType) as IEnumerable<object>;

        if (handlers == null || !handlers.Any())
        {
            return;
        }

        var method = GetHandlerMethod(exportEvent);

        if (method == null)
        {
            return;
        }

        await method(handlers, exportEvent, token);
    }

    private static async Task HandleGenericEventAsync<T>(IEnumerable<IExportEventHandler<T>> handlers, IExportEvent<T> exportEvent, CancellationToken token)
    {
        foreach (var handler in handlers)
        {
            await handler.HandleEventAsync(exportEvent, token);
        }
    }

    private static Type? GetHandlerType(IExportEvent exportEvent)
    {
        return EventHandlerTypeMap.GetOrAdd(exportEvent.GetType(), type =>
        {
            var payloadType = GetPayloadType(type);
            if (payloadType == null)
            {
                return null;
            }

            var handlerType = typeof(IExportEventHandler<>).MakeGenericType(payloadType);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerType);

            return enumerableType;
        });
    }

    private static GenericHandlerDelegate? GetHandlerMethod(IExportEvent exportEvent)
    {
        return HandlerMethodMap.GetOrAdd(exportEvent.GetType(), type =>
        {
            var payloadType = GetPayloadType(type);
            if (payloadType == null)
            {
                return null;
            }

            var handlersType = typeof(IEnumerable<>).MakeGenericType(
                typeof(IExportEventHandler<>).MakeGenericType(payloadType)
            );
            var eventType = typeof(IExportEvent<>).MakeGenericType(payloadType);
            var method = HandleGenericEventMethod.MakeGenericMethod(payloadType);

            var handlersParam = Expression.Parameter(typeof(object), "handlers");
            var eventParam = Expression.Parameter(typeof(IExportEvent), "exportEvent");
            var tokenParam = Expression.Parameter(typeof(CancellationToken), "token");

            var castHandlers = Expression.Convert(handlersParam, handlersType);
            var castEvent = Expression.Convert(eventParam, eventType);

            var callExpression = Expression.Call(method, castHandlers, castEvent, tokenParam);
            var lambda = Expression.Lambda<GenericHandlerDelegate>(callExpression, handlersParam, eventParam, tokenParam);

            return lambda.Compile();
        });
    }

    private static Type? GetPayloadType(Type type)
    {
        if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(ExportEvent<>))
        {
            return null;
        }

        return type.GetGenericArguments()[0];
    }
}