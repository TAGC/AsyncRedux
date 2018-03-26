using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TimeItCore;

namespace AsyncRedux.Middleware.Logging
{
    /// <summary>
    /// Extends <see cref="StoreSetup.Builder{TState}" /> with methods to configure logging middleware.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Configures the store with middleware to log dispatched actions. The middleware will be configured with its default
        /// settings.
        /// </summary>
        /// <typeparam name="TState">The type of the state handled by the store.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="loggerFactory">A factory to generate the logger to log dispatched actions with.</param>
        /// <returns>This builder.</returns>
        /// <seealso cref="LoggingMiddlewareOptions.Default" />
        public static StoreSetup.Builder<TState> UsingLoggingMiddleware<TState>(
            this StoreSetup.Builder<TState> builder,
            ILoggerFactory loggerFactory)
        {
            return builder.UsingLoggingMiddleware(loggerFactory, LoggingMiddlewareOptions.Default);
        }

        /// <summary>
        /// Configures the store with middleware to log dispatched actions.
        /// </summary>
        /// <typeparam name="TState">The type of the state handled by the store.</typeparam>
        /// <param name="builder">This builder.</param>
        /// <param name="loggerFactory">A factory to generate the logger to log dispatched actions with.</param>
        /// <param name="options">Configuration settings for the middleware.</param>
        /// <returns>This builder.</returns>
        public static StoreSetup.Builder<TState> UsingLoggingMiddleware<TState>(
            this StoreSetup.Builder<TState> builder,
            ILoggerFactory loggerFactory,
            LoggingMiddlewareOptions options)
        {
            if (options.LogLevel == LogLevel.None)
            {
                // Pass through.
                return builder.UsingMiddleware(store => next => next);
            }

            var logger = loggerFactory.CreateLogger<IStore<TState>>();
            var log = GetLogMethod(logger, options.LogLevel);

            Func<Dispatcher, Dispatcher> LogDispatches(IStore<TState> store) => next => async action =>
            {
                if (options.LogAtStart)
                {
                    log("Dispatching: {Action}", new[] { action });
                }

                if (options.LogElapsedTime)
                {
                    using (TimeIt.Then.Log(logger, options.LogLevel, options.ElapsedTimeLogTemplate))
                    {
                        await next(action);
                    }
                }
                else
                {
                    await next(action);
                }

                object nextState = store.State;

                if (options.LogNextState)
                {
                    log("Next state: {State}", new[] { nextState });
                }

                return Task.CompletedTask;
            };

            return builder.UsingMiddleware(LogDispatches);
        }

        private static Action<string, object[]> GetLogMethod(ILogger logger, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace: return logger.LogTrace;
                case LogLevel.Debug: return logger.LogDebug;
                case LogLevel.Information: return logger.LogInformation;
                case LogLevel.Warning: return logger.LogWarning;
                case LogLevel.Error: return logger.LogError;
                case LogLevel.Critical: return logger.LogCritical;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}