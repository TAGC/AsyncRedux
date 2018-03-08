using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncRedux.Tests.Mock;
using AsyncRedux.Tests.Mock.Actions;
using Shouldly;
using Xunit;

// ReSharper disable MemberCanBePrivate.Global

namespace AsyncRedux.Tests
{
    public class StoreSpec
    {
        public static IEnumerable<object[]> DispatchExamples
        {
            get
            {
                yield return new object[]
                {
                    new State(0, false),
                    new object[] { },
                    new State(0, false)
                };

                yield return new object[]
                {
                    new State(1, false),
                    new object[] { new ChangeBool(true) },
                    new State(1, true)
                };

                yield return new object[]
                {
                    new State(100, true),
                    new object[] { new ChangeInt(50), new ChangeBool(false) },
                    new State(50, false)
                };

                yield return new object[]
                {
                    new State(5, false),
                    new object[] { new ChangeInt(10), new ChangeBool(false), new ChangeInt(20) },
                    new State(20, false)
                };

                yield return new object[]
                {
                    new State(5, false),
                    new[] { new ChangeInt(10), new ChangeBool(false), new object() },
                    new State(10, false)
                };
            }
        }

        public static IEnumerable<object[]> StateExamples
        {
            get
            {
                yield return new object[] { new State() };
                yield return new object[] { new State(1, false) };
                yield return new object[] { new State(0, true) };
                yield return new object[] { new State(1, true) };
            }
        }

        [Theory]
        [MemberData(nameof(DispatchExamples))]
        internal async Task Store_Should_Apply_Actions_In_Order_Of_Dispatch(
            State initialState,
            object[] actions,
            State expectedFinalState)
        {
            var store = StoreSetup.CreateStore(Reducers.Replace, initialState);

            foreach (var action in actions)
            {
                await store.Dispatch(action);
            }

            store.State.ShouldBe(expectedFinalState);
        }

        [Theory]
        [MemberData(nameof(StateExamples))]
        internal void Store_Should_Have_Provided_Initial_State_After_Construction(State initialState)
        {
            var store = StoreSetup.CreateStore(Reducers.Replace, initialState);
            store.State.ShouldBe(initialState);
        }
    }
}