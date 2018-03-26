using System;
using AsyncRedux.Tests.Mock.Actions;

namespace AsyncRedux.Tests.Mock
{
    public static class Middleware
    {
        public static Func<Dispatcher, Dispatcher> IncrementInt(IStore<State> store) => next => action =>
        {
            if (!(action is ChangeInt changeInt))
            {
                return next(action);
            }

            return next(new ChangeInt(changeInt.NewInt + 1));
        };

        public static Func<Dispatcher, Dispatcher> NegateBool(IStore<State> store) => next => action =>
        {
            if (!(action is ChangeBool changeBool))
            {
                return next(action);
            }

            return next(new ChangeBool(!changeBool.NewBool));
        };
    }
}