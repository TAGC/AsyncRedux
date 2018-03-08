using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AsyncRedux
{
    /// <summary>
    /// Represents a function that asynchronously dispatches an action.
    /// </summary>
    /// <param name="action">The action to dispatch.</param>
    /// <returns>Different delegates of this type may return various values.</returns>
    [PublicAPI]
    public delegate Task<object> Dispatcher([NotNull] object action);

    /// <summary>
    /// Represents a function used as middleware within the store.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <param name="store">The store this middleware is used by.</param>
    /// <returns>A function that accepts a dispatcher, wraps it and returns the wrapper dispatcher.</returns>
    [PublicAPI]
    public delegate Func<Dispatcher, Dispatcher> Middleware<in TState>([NotNull] IStore<TState> store);

    /// <summary>
    /// Represents a pure function that takes a state and an action and yields a new state.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <param name="state">The current state.</param>
    /// <param name="action">The action to apply.</param>
    /// <returns>The new state.</returns>
    [PublicAPI]
    [CanBeNull]
    public delegate TState Reducer<TState>([CanBeNull] TState state, [NotNull] object action);
}