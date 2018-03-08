using System;
using JetBrains.Annotations;

namespace AsyncRedux
{
    /// <summary>
    /// Represents a type of <see cref="IStore{TState}" /> that can notify clients when actions are dispatched to it.
    /// </summary>
    /// <typeparam name="TState">The type of the state maintained by this store.</typeparam>
    [PublicAPI]
    public interface IObservableStore<out TState> : IStore<TState>
    {
        /// <summary>
        /// Allows actions dispatched to this store to be observed.
        /// </summary>
        /// <returns>An observable that dispatched actions can be observed on.</returns>
        IObservable<object> Observe();
    }
}