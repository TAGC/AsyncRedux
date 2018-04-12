using System;

namespace Chess.Pieces
{
    internal class Queen : Piece
    {
        /// <inheritdoc />
        public Queen(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override char BlackSymbol { get; } = '♛';

        /// <inheritdoc />
        protected override char WhiteSymbol { get; } = '♕';

        /// <inheritdoc />
        public override Board ApplyMove(Board board, Move move)
        {
            // A queen acts like a combination of a bishop and a rook.
            
            var factory = Owner == Player.White ? Factory.White : Factory.Black;
            var bishop = factory.Bishop;
            var rook = factory.Rook;

            foreach (var piece in new[] { bishop, rook })
            {
                try
                {
                    return piece.ApplyMove(board, move);
                }
                catch (InvalidMoveException)
                {
                    continue;
                }
            }

            throw new InvalidMoveException($"Cannot move to {move.To}");
        }
    }
}