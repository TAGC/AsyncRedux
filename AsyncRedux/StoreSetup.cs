using System;
using JetBrains.Annotations;

namespace AsyncRedux
{
    /// <summary>
    /// Used to configure and return a new Redux store.
    /// </summary>
    /// <remarks>
    /// At present, no configuration is possible for the store. This class exists to support future versions
    /// of this project that may allow for configuration, but more importantly to ensure clients access the
    /// bus through an interface and not a direct class.
    /// </remarks>
    [PublicAPI]
    public static class StoreSetup
    {
        /// <summary>
        /// Creates and returns a new store.
        /// </summary>
        /// <typeparam name="TState">The type of the state handled by the store.</typeparam>
        /// <param name="reducer">The root reducer to apply to dispatched actions.</param>
        /// <param name="initialState">The initial state.</param>
        /// <returns>A new store.</returns>
        public static IObservableStore<TState> CreateStore<TState>(
            [NotNull] Reducer<TState> reducer,
            [CanBeNull] TState initialState = default)
        {
            throw new NotImplementedException();
        }
    }
}