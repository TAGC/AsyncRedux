using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AsyncRedux.Tests.Mock;
using AsyncRedux.Tests.Mock.Actions;
using Shouldly;
using Xunit;

// ReSharper disable MemberCanBePrivate.Global

namespace AsyncRedux.Tests
{
    using ObservableSetup = Func<IObservable<object>, IObservable<object>>;

    public class ObservableStoreSpec
    {
        private readonly IObservableStore<State> _store;

        /// <inheritdoc />
        public ObservableStoreSpec()
        {
            _store = StoreSetup.CreateStore<State>(Reducers.PassThrough);
        }

        public static IEnumerable<object[]> ActionSequenceExamples
        {
            get
            {
                yield return new object[]
                {
                    new object[] { new ChangeInt(3), new ChangeBool(true), new ChangeBool(true), new ChangeInt(3) },
                    new ObservableSetup(observable => observable),
                    new object[] { new ChangeInt(3), new ChangeBool(true), new ChangeBool(true), new ChangeInt(3) }
                };

                yield return new object[]
                {
                    new object[] { new ChangeInt(3), new ChangeBool(true), new ChangeBool(true), new ChangeInt(3) },
                    new ObservableSetup(observable => observable.Distinct()),
                    new object[] { new ChangeInt(3), new ChangeBool(true) }
                };

                yield return new object[]
                {
                    new object[] { new ChangeInt(3), new ChangeBool(true), new ChangeBool(true), new ChangeInt(3) },
                    new ObservableSetup(observable => observable.DistinctUntilChanged()),
                    new object[] { new ChangeInt(3), new ChangeBool(true), new ChangeInt(3) }
                };

                yield return new object[]
                {
                    new[] { new ChangeInt(3), new ChangeBool(true), new object(), new ChangeInt(5) },
                    new ObservableSetup(observable => observable.Where(it => it is ChangeInt)),
                    new object[] { new ChangeInt(3), new ChangeInt(5) }
                };
            }
        }

        [Theory]
        [MemberData(nameof(ActionSequenceExamples))]
        internal async Task Observable_Store_Should_Action_Observation(
            object[] actionSequence,
            ObservableSetup setup,
            object[] expectedObservedActions)
        {
            var observedActions = new List<object>();

            using (setup(_store.Observe()).Subscribe(observedActions.Add))
            {
                foreach (var action in actionSequence)
                {
                    await _store.Dispatch(action);
                }
            }

            observedActions.ShouldBe(expectedObservedActions);
        }

        [Fact]
        internal async Task Observable_Store_Should_Support_Multiple_Observers()
        {
            var actions = new object[] { new ChangeInt(4), new ChangeBool(true) };
            var observedActionsA = new List<object>();
            var observedActionsB = new List<object>();

            using (_store.Observe().Subscribe(observedActionsA.Add))
            using (_store.Observe().Subscribe(observedActionsB.Add))
            {
                foreach (var action in actions)
                {
                    await _store.Dispatch(action);
                }

                observedActionsA.ShouldBe(actions);
                observedActionsB.ShouldBe(actions);
            }
        }

        [Fact]
        internal async Task Observers_Should_Not_Receive_Dispatched_Actions_After_Disposing_Subscription()
        {
            var changeInt = new ChangeInt(5);
            var changeBool = new ChangeBool(true);
            var observedActionsA = new List<object>();
            var observedActionsB = new List<object>();
            var subscriptionA = _store.Observe().Subscribe(observedActionsA.Add);
            var subscriptionB = _store.Observe().Subscribe(observedActionsB.Add);

            await _store.Dispatch(changeInt);
            observedActionsA.ShouldBe(new object[] { changeInt });
            observedActionsB.ShouldBe(new object[] { changeInt });

            subscriptionA.Dispose();
            await _store.Dispatch(changeBool);
            observedActionsA.ShouldBe(new object[] { changeInt });
            observedActionsB.ShouldBe(new object[] { changeInt, changeBool });

            subscriptionB.Dispose();
            await _store.Dispatch(new object());
            observedActionsA.ShouldBe(new object[] { changeInt });
            observedActionsB.ShouldBe(new object[] { changeInt, changeBool });
        }
    }
}