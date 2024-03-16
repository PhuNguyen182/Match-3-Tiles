using MessagePipe;
using VContainer;

namespace TestMessagePipe
{
    public static class MessageBrokerHelper
    {
        // Register IPublisher<T>/ISubscriber<T> and global filter.
        public static void RegisterMessageBroker<T>(IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterMessageBroker<T>(options);

            // setup for global filters.
            options.AddGlobalMessageHandlerFilter<MessageHandlerFilter<T>>();
        }

        // Register IRequestHandler<TReq, TRes>/IRequestAllHandler<TReq, TRes> and global filter.
        public static void RegisterRequest<TRequest, TResponse, THandler>(IContainerBuilder builder, MessagePipeOptions options)
            where THandler : IRequestHandler
        {
            builder.RegisterRequestHandler<TRequest, TResponse, THandler>(options);

            // setup for global filters.
            options.AddGlobalRequestHandlerFilter<RequestHandlerFilter<TRequest, TResponse>>();
        }
    }
}
