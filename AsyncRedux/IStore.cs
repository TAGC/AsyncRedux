using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AsyncRedux
{
    /// <summary>
    /// Represents a store that maintains the single source of truth of a particular type of state and allows that state to be
    /// modified in response to dispatched actions.
    /// </summary>
    /// <typeparam name="TState">The type of the state maintained in this store.</typeparam>
    [PublicAPI]
    public interface IStore<out TState>
    {
        /// <summary>
        /// Gets the current state.
        /// </summary>
        TState State { get; }

        /// <summary>
        /// Dispatches the specified action to this store.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>No object or value is returned by this method when it returns.</returns>
        Task Dispatch([NotNull] object action);
    }
}