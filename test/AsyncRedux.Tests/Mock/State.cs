namespace AsyncRedux.Tests.Mock
{
    public struct State
    {
        public State(int intProperty, bool boolProperty)
        {
            IntProperty = intProperty;
            BoolProperty = boolProperty;
        }

        public bool BoolProperty { get; }

        public int IntProperty { get; }

        public override string ToString() => $"({IntProperty}, {BoolProperty})";
    }
}