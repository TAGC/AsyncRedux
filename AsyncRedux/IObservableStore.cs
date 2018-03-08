using System;
using System.Threading.Tasks;
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

        /// <summary>
        /// Registers a subscriber to dispatched actions of type <typeparamref name="TAction" />.
        /// </summary>
        /// <param name="callback">
        /// The asynchronous callback to invoke when an action of the requested type is dispatched.
        /// </param>
        /// <typeparam name="TAction">The type of actions to subscribe to.</typeparam>
        /// <returns>A token that can be disposed to unregister this subscriber.</returns>
        IDisposable Subscribe<TAction>(Func<TAction, Task> callback);

        /// <summary>
        /// Registers a(n) <see cref="IStoreSubscriber{TStore,TAction}" /> to dispatched actions of type
        /// <typeparamref name="TAction" />.
        /// <para></para>
        /// During this call the store will pass itself to the subscriber through
        /// <see cref="IStoreSubscriber{TStore,TAction}.SetStore" />
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <typeparam name="TAction">The type of actions to subscribe to.</typeparam>
        /// <returns>
        /// A token that can be disposed to unregister this subscriber.
        /// </returns>
        IDisposable Subscribe<TAction>(IStoreSubscriber<TState, TAction> subscriber);
    }
}