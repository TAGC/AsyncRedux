using System;

namespace Chess.Pieces
{
    internal class King : Piece
    {
        /// <inheritdoc />
        public King(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override char BlackSymbol { get; } = '♚';

        /// <inheritdoc />
        protected override char WhiteSymbol { get; } = '♔';

        /// <inheritdoc />
        public override Board ApplyMove(Board board, Move move) => throw new NotImplementedException();
    }
}