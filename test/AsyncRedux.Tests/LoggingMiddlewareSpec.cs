using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AsyncRedux.Middleware.Logging;
using AsyncRedux.Tests.Mock;
using AsyncRedux.Tests.Mock.Actions;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace AsyncRedux.Tests
{
    public class LoggingMiddlewareSpec
    {
        private readonly LoggerFactory _loggerFactory;
        private readonly List<string> _logs;

        /// <inheritdoc />
        public LoggingMiddlewareSpec()
        {
            _logs = new List<string>();
            _loggerFactory = new LoggerFactory();
            _loggerFactory.Logger.GeneratedLog += (s, e) => _logs.Add(e.Log);
        }

        [Theory]
        [CombinatorialData]
        internal async Task Middleware_Should_Be_Configurable_To_Log_Dispatch_Execution_Time(
            [CombinatorialValues("Dispatch took {Elapsed}", "Action handled in {Time}")]
            string template)
        {
            // Given: a store which is configured to use logging middleware
            var options = LoggingMiddlewareOptions.Default;
            options.LogAtStart = false;
            options.LogNextState = false;
            options.LogElapsedTime = true;
            options.ElapsedTimeLogTemplate = template;

            var store = CreateStore(options);

            // When: we dispatch an action
            await store.Dispatch("foo");

            // Then: the execution time of the action should have been logged
            var expectedLogFormat = Regex.Replace(template, @"{.*}", @"\d{2}:\d{2}:\d{2}\.\d{7}");
            _logs.ShouldHaveSingleItem().ShouldMatch(expectedLogFormat);
        }

        [Fact]
        internal async Task Middleware_Should_Be_Configurable_To_Log_Next_State_After_Dispatch()
        {
            // Given: a store which is configured to use logging middleware.
            var options = LoggingMiddlewareOptions.Default;
            options.LogAtStart = false;
            options.LogElapsedTime = false;
            options.LogNextState = true;

            var store = StoreSetup.CreateStore<State>()
                .FromReducer(Reducers.Replace)
                .WithInitialState(new State(0, false))
                .UsingLoggingMiddleware(_loggerFactory, options)
                .Build();

            // When: we dispatch an action to change the state.
            await store.Dispatch(new ChangeInt(5));

            // Then: the new state resulting from the action should have been logged.
            var expectedNextState = new State(5, false);
            _logs.ShouldHaveSingleItem().ShouldContain(expectedNextState.ToString());
        }

        [Theory]
        [CombinatorialData]
        internal async Task Middleware_Should_Be_Configurable_To_Log_Start_Of_Dispatch(bool logAtStart)
        {
            // Given: a store which is configured to use logging middleware
            var options = LoggingMiddlewareOptions.Default;
            options.LogAtStart = logAtStart;
            options.LogElapsedTime = false;
            options.LogNextState = false;

            var store = CreateStore(options);

            // When: we start dispatching an action but don't let it complete.
            var tcs = new TaskCompletionSource<object>();
            store.Subscribe<object>(action => tcs.Task);
            var _ = store.Dispatch("foo");
            await Task.Delay(100);

            // Then: a log should have been generated iff the middleware should log the start of dispatches.
            _logs.Count.ShouldBe(logAtStart ? 1 : 0);
        }

        [Theory]
        [CombinatorialData]
        internal async Task Middleware_Should_Log_At_Specified_Log_Level(LogLevel logLevel)
        {
            // Given: a store which is configured to use logging middleware
            var options = LoggingMiddlewareOptions.Default;
            options.LogAtStart = true;
            options.LogElapsedTime = true;
            options.LogLevel = logLevel;

            var store = CreateStore(options);

            // When: we dispatch an action
            await store.Dispatch("foo");

            // Then: logs should have been generated only at the configured log level
            var actualLogLevels = _logs
                .Select(it => Regex.Match(it, @"\[(?<enum>.*)\]").Groups["enum"].Value)
                .Select(Enum.Parse<LogLevel>)
                .ToList();

            if (logLevel == LogLevel.None)
            {
                actualLogLevels.ShouldBeEmpty();
            }
            else
            {
                actualLogLevels.ShouldNotBeEmpty();
                actualLogLevels.ShouldAllBe(it => it == logLevel);
            }
        }

        private IObservableStore<State> CreateStore(LoggingMiddlewareOptions options)
        {
            return StoreSetup.CreateStore<State>()
                .FromReducer(Reducers.PassThrough)
                .UsingLoggingMiddleware(_loggerFactory, options)
                .Build();
        }
    }
}