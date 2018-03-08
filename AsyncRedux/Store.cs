using System;
using System.Linq;
using System.Threading.Tasks;
using AsyncBus;
using JetBrains.Annotations;

namespace AsyncRedux
{
    /// <inheritdoc />
    internal sealed class Store<TState> : IObservableStore<TState>
    {
        private readonly IBus _bus;
        private readonly Dispatcher _dispatcher;
        private readonly Reducer<TState> _reducer;

        /// <inheritdoc />
        public Store(
            [NotNull] Reducer<TState> reducer,
            [CanBeNull] TState initialState = default,
            [NotNull] params Middleware<TState>[] middleware)
        {
            _reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
            _dispatcher = CreateDispatcher(middleware ?? throw new ArgumentNullException(nameof(middleware)));
            _bus = BusSetup.CreateBus();
            State = initialState;
        }

        /// <inheritdoc />
        public TState State { get; private set; }

        /// <inheritdoc />
        public Task Dispatch(object action) => _dispatcher(action);

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

        private Dispatcher CreateDispatcher(Middleware<TState>[] middleware)
        {
            async Task<object> InnerDispatch(object action)
            {
                State = _reducer(State, action);
                await _bus.Publish(action);
                return action;
            }

            Dispatcher ApplyMiddleware(Dispatcher dispatcher, Middleware<TState> m) => m(this)(dispatcher);

            return middleware.Aggregate<Middleware<TState>, Dispatcher>(InnerDispatch, ApplyMiddleware);
        }
    }
}