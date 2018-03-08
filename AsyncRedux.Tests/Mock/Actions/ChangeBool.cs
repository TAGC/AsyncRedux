namespace AsyncRedux.Tests.Mock.Actions
{
    public struct ChangeBool
    {
        public ChangeBool(bool newBool)
        {
            NewBool = newBool;
        }

        public bool NewBool { get; }

        public override string ToString() => $"bool=>{NewBool}";
    }
}