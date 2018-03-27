using System;
using Microsoft.Extensions.Logging;

namespace AsyncRedux.Tests.Mock
{
    public class LoggerFactory : ILoggerFactory
    {
        public Logger Logger { get; } = new Logger();

        /// <inheritdoc />
        public void AddProvider(ILoggerProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName) => Logger;

        /// <inheritdoc />
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}