using Microsoft.Extensions.Logging;

namespace AsyncRedux.Middleware.Logging
{
    /// <summary>
    /// Configuration settings for the logging middleware.
    /// </summary>
    public struct LoggingMiddlewareOptions
    {
        private static LoggingMiddlewareOptions _defaultOptions = new LoggingMiddlewareOptions
        {
            ElapsedTimeLogTemplate = "Action handled in {Elapsed}",
            LogAtStart = false,
            LogLevel = LogLevel.Trace,
            LogElapsedTime = true,
            LogNextState = true
        };

        /// <summary>
        /// Gets the default configuration settings for the logging middleware.
        /// </summary>
        public static ref readonly LoggingMiddlewareOptions Default => ref _defaultOptions;

        /// <summary>
        /// Gets or sets the template for logging the execution time of dispatched actions. This is only applicable if
        /// <see cref="LogElapsedTime" /> is <c>true</c>.
        /// </summary>
        public string ElapsedTimeLogTemplate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to log the action before forwarding it down the middleware chain.
        /// </summary>
        public bool LogAtStart { get; set; }

        /// <summary>
        /// Gets or sets the level to log at.
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to log the time taken to process the action.
        /// </summary>
        public bool LogElapsedTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to log the resulting state following dispatch of an action.
        /// </summary>
        public bool LogNextState { get; set; }
    }
}