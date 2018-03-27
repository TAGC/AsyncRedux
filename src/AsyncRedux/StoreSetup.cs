using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace AsyncRedux
{
    /// <summary>
    /// Used to configure and return a new Redux store.
    /// </summary>
    [PublicAPI]
    public static class StoreSetup
    {
        /// <summary>
        /// Returns a <see cref="Builder{TState}" /> to configure and create a new store.
        /// </summary>
        /// <typeparam name="TState">The type of the state handled by the store.</typeparam>
        /// <returns>A <see cref="Builder{TState}" />.</returns>
        public static Builder<TState> CreateStore<TState>() => new Builder<TState>();

        /// <summary>
        /// Configures and builds instances of <see cref="IObservableStore{TState}" />.
        /// </summary>
        /// <typeparam name="TState">The type of the state handled by the store.</typeparam>
        /// <remarks>
        /// Middleware plugin libraries can and should extend this class with methods to configure middleware they provide.
        /// </remarks>
        public class Builder<TState>
        {
            private readonly List<Middleware<TState>> _middleware;

            private TState _initialState;
            private Reducer<TState> _reducer;

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder{TState}" /> class.
            /// </summary>
            public Builder()
            {
                _middleware = new List<Middleware<TState>>();
            }

            /// <summary>
            /// Builds and returns a new store based on the configuration of this builder.
            /// </summary>
            /// <returns>A new store.</returns>
            /// <exception cref="InvalidOperationException">This builder has not been configured correctly.</exception>
            public IObservableStore<TState> Build()
            {
                if (_reducer is null)
                {
                    throw new InvalidOperationException("Root reducer for store has not been configured");
                }

                return new Store<TState>(_reducer, _initialState, _middleware);
            }

            /// <summary>
            /// Configures the root reducer for the store. This <b>must</b> be set before building the store.
            /// </summary>
            /// <param name="reducer">The root reducer to apply to dispatched actions.</param>
            /// <returns>This builder.</returns>
            public Builder<TState> FromReducer([NotNull] Reducer<TState> reducer)
            {
                _reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
                return this;
            }

            /// <summary>
            /// Configures the middleware to apply to the store. Middleware will be applied in the order that it is provided.
            /// </summary>
            /// <param name="middleware">The middleware.</param>
            /// <returns>This builder.</returns>
            public Builder<TState> UsingMiddleware([NotNull] [ItemNotNull] params Middleware<TState>[] middleware)
            {
                if (middleware is null)
                {
                    throw new ArgumentNullException(nameof(middleware));
                }

                if (middleware.Any(it => it is null))
                {
                    throw new ArgumentException("Middleware delegate cannot be null", nameof(middleware));
                }

                _middleware.AddRange(middleware);
                return this;
            }

            /// <summary>
            /// Configures the initial state for the store. If not specified, defaults to the default value for
            /// <typeparamref name="TState" />.
            /// </summary>
            /// <param name="initialState">The initial state.</param>
            /// <returns>This builder.</returns>
            public Builder<TState> WithInitialState([CanBeNull] TState initialState)
            {
                _initialState = initialState;
                return this;
            }
        }
    }
}