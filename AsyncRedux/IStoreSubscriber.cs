using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AsyncRedux
{
    /// <summary>
    /// Represents an object that can subscribe to actions dispatched on a(n) <see cref="IStore{TState}" />.
    /// </summary>
    /// <typeparam name="TStore">The type of the store to subscribe to.</typeparam>
    /// <typeparam name="TAction">The type of the action to process.</typeparam>
    [PublicAPI]
    public interface IStoreSubscriber<in TStore, in TAction>
    {
        /// <summary>
        /// Asynchronously handles the dispatched action.
        /// </summary>
        /// <param name="action">The dispatched action.</param>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        Task ProcessDispatchedAction([NotNull] TAction action);

        /// <summary>
        /// Sets the store that this instance is subscribed to.
        /// <para></para>
        /// This method will be invoked when subscribing to a(n) <see cref="IObservableStore{TState}" />, where the store will pass
        /// itself. This allows the subscriber to fetch the current state and dispatch new actions.
        /// </summary>
        /// <param name="store">The store.</param>
        void SetStore([NotNull] IStore<TStore> store);
    }
}