using System;
using System.Threading.Tasks;
using AsyncBus;
using JetBrains.Annotations;

namespace AsyncRedux
{
    /// <inheritdoc />
    internal sealed class Store<TState> : IObservableStore<TState>
    {
        private readonly IBus _bus;
        private readonly Reducer<TState> _reducer;

        /// <inheritdoc />
        public Store(
            [NotNull] Reducer<TState> reducer,
            [CanBeNull] TState initialState = default)
        {
            _reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
            _bus = BusSetup.CreateBus();
            State = initialState;
        }

        /// <inheritdoc />
        public TState State { get; private set; }

        /// <inheritdoc />
        public Task Dispatch(object action)
        {
            State = _reducer(State, action);

            return _bus.Publish(action);
        }

        /// <inheritdoc />
        public IObservable<object> Observe() => _bus.Observe<object>();

        /// <inheritdoc />
        public IDisposable Subscribe<TAction>(Func<TAction, Task> callback) => _bus.Subscribe(callback);

        /// <inheritdoc />
        public IDisposable Subscribe<TAction>(IStoreSubscriber<TState, TAction> subscriber)
        {
            subscriber.SetStore(this);

            return _bus.Subscribe<TAction>(subscriber.ProcessDispatchedAction);
        }
    }
}