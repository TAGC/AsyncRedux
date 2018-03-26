using System;
using System.Threading.Tasks;

namespace AsyncRedux.Tests.Mock
{
    public class Subscriber : IStoreSubscriber<State, object>
    {
        public IStore<State> Store { get; private set; }

        /// <inheritdoc />
        public Task ProcessDispatchedAction(object action)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetStore(IStore<State> store)
        {
            Store = store;
        }
    }
}