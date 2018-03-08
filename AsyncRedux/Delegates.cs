using JetBrains.Annotations;

namespace AsyncRedux
{
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