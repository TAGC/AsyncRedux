using System;

namespace Chess
{
    internal readonly struct Position
    {
        public const int MinRank = 1;
        public const int MaxRank = 8;
        public const ChessFile MinFile = ChessFile.A;
        public const ChessFile MaxFile = ChessFile.H;

        public Position(ChessFile file, int rank)
        {
            if (file < MinFile || file > MaxFile)
            {
                throw new ArgumentOutOfRangeException(nameof(file));
            }

            if (rank < MinRank || rank > MaxRank)
            {
                throw new ArgumentOutOfRangeException(nameof(rank));
            }
            
            File = file;
            Rank = rank;
        }

        public static bool TryCreate(ChessFile file, int rank, out Position? position)
        {
            if (file < MinFile || file > MaxFile || rank < MinRank || rank > MaxRank)
            {
                position = null;
                return false;
            }

            position = new Position(file, rank);
            return true;
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

        public void Deconstruct(out ChessFile file, out int rank)
        {
            file = File;
            rank = Rank;
        }

        /// <inheritdoc />
        public override string ToString() => $"{File}{Rank}";
    }
}