using AsyncRedux.Tests.Mock.Actions;

namespace AsyncRedux.Tests.Mock
{
    public static class Reducers
    {
        public static State PassThrough(State state, object action) => state;

        public static State Replace(State state, object action)
        {
            switch (action)
            {
                case ChangeInt changeInt: return new State(changeInt.NewInt, state.BoolProperty);
                case ChangeBool changeBool: return new State(state.IntProperty, changeBool.NewBool);
                default: return state;
            }
        }
    }
}