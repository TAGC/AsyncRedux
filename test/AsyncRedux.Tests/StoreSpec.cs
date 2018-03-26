using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncRedux.Tests.Mock;
using AsyncRedux.Tests.Mock.Actions;
using Shouldly;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute
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

        [Fact]
        internal void Store_Construction_Should_Throw_For_Null_Middleware_Collection()
        {
            Should.Throw<ArgumentNullException>(
                () => StoreSetup.CreateStore<State>()
                    .FromReducer(Reducers.PassThrough)
                    .UsingMiddleware(null)
                    .Build());
        }

        [Fact]
        internal void Store_Construction_Should_Throw_If_Any_Middleware_Is_Null()
        {
            Should.Throw<ArgumentException>(
                () => StoreSetup.CreateStore<State>()
                    .FromReducer(Reducers.PassThrough)
                    .UsingMiddleware(Middleware.IncrementInt, null)
                    .Build());
        }

        [Fact]
        internal void Store_Construction_Should_Throw_If_Reducer_Is_Not_Specified()
        {
            Should.Throw<InvalidOperationException>(() => StoreSetup.CreateStore<State>().Build());
        }

        [Theory]
        [MemberData(nameof(DispatchExamples))]
        internal async Task Store_Should_Apply_Actions_In_Order_Of_Dispatch(
            State initialState,
            object[] actions,
            State expectedFinalState)
        {
            var store = StoreSetup.CreateStore<State>()
                .FromReducer(Reducers.Replace)
                .WithInitialState(initialState)
                .Build();

            foreach (var action in actions)
            {
                await store.Dispatch(action);
            }

            store.State.ShouldBe(expectedFinalState);
        }

        [Fact]
        internal void Store_Should_Concatenate_Middleware_If_Multiple_Sets_Provided_During_Construction()
        {
            var initialState = new State(0, false);
            var store = StoreSetup.CreateStore<State>()
                .FromReducer(Reducers.Replace)
                .WithInitialState(initialState)
                .UsingMiddleware(Middleware.IncrementInt)
                .UsingMiddleware(Middleware.NegateBool)
                .Build();

            store.Dispatch(new ChangeInt(1));
            store.State.IntProperty.ShouldBe(2);

            store.Dispatch(new ChangeBool(false));
            store.State.BoolProperty.ShouldBe(true);
        }

        [Theory]
        [MemberData(nameof(StateExamples))]
        internal void Store_Should_Have_Provided_Initial_State_After_Construction(State initialState)
        {
            var store = StoreSetup.CreateStore<State>()
                .FromReducer(Reducers.Replace)
                .WithInitialState(initialState)
                .Build();

            store.State.ShouldBe(initialState);
        }

        [Fact]
        internal void Store_Should_Pass_Dispatched_Actions_Through_Middleware_Before_Reducing()
        {
            var initialState = new State(0, false);
            var middleware = new Middleware<State>[] { Middleware.IncrementInt, Middleware.NegateBool };
            var store = StoreSetup.CreateStore<State>()
                .FromReducer(Reducers.Replace)
                .WithInitialState(initialState)
                .UsingMiddleware(middleware)
                .Build();

            store.Dispatch(new ChangeInt(1));
            store.State.IntProperty.ShouldBe(2);

            store.Dispatch(new ChangeBool(false));
            store.State.BoolProperty.ShouldBe(true);
        }
    }
}