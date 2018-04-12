using static Chess.Position;

namespace Chess.Pieces
{
    internal abstract class Piece : IPiece
    {
        protected Piece(Player owner)
        {
            Owner = owner;
        }

        /// <inheritdoc />
        public Player Owner { get; }

        /// <inheritdoc />
        public char Symbol => Owner == Player.White ? WhiteSymbol : BlackSymbol;

        protected abstract char BlackSymbol { get; }

        protected abstract char WhiteSymbol { get; }

        public abstract Board ApplyMove(Board board, Move move);

        protected bool OpposingPieceAt(Board board, ChessFile file, int rank)
        {
            if (!Position.TryCreate(file, rank, out var position))
            {
                return false;
            }

            return board[position.Value] is IPiece otherPiece && otherPiece.Owner != Owner;
        }

        protected bool FreeSpaceAt(Board board, ChessFile file, int rank)
        {
            if (!Position.TryCreate(file, rank, out var position))
            {
                return false;
            }

            return board[position.Value] is null;
        }
    }
}