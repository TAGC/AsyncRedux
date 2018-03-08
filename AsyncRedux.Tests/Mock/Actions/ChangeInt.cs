namespace AsyncRedux.Tests.Mock.Actions
{
    public struct ChangeInt
    {
        public ChangeInt(int newInt)
        {
            NewInt = newInt;
        }

        public int NewInt { get; }

        public override string ToString() => $"int=>{NewInt}";
    }
}