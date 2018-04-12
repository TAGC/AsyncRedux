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

        public abstract bool CanApplyMove(Board currentBoard, Move move);

        /// <inheritdoc />
        Board IPiece.ApplyMove(Board board, Move move) =>
            !CanApplyMove(board, move) ? board : ApplyMove(board, move);
    }
}