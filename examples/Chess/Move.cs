namespace Chess
{
    internal readonly struct Move
    {
        public Move(Player player, Position from, Position to)
        {
            Player = player;
            From = from;
            To = to;
        }

        public Position From { get; }

        public Player Player { get; }

        public Position To { get; }

        /// <inheritdoc />
        public override string ToString() => $"{Player}: {From} to {To}";
    }
}