namespace Chess
{
    internal readonly struct Position
    {
        public Position(ChessFile file, int rank)
        {
            File = file;
            Rank = rank;
        }

        public enum ChessFile
        {
            A = 1,
            B,
            C,
            D,
            E,
            F,
            G,
            H
        }

        public ChessFile File { get; }

        public int Rank { get; }

        /// <inheritdoc />
        public override string ToString() => $"{File}{Rank}";
    }
}