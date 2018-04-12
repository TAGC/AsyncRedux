using System;
using System.Linq;
using static Chess.Position;

namespace Chess.Pieces
{
    internal class Knight : Piece
    {
        /// <inheritdoc />
        public Knight(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override char BlackSymbol { get; } = '♞';

        /// <inheritdoc />
        protected override char WhiteSymbol { get; } = '♘';

        /// <inheritdoc />
        public override Board ApplyMove(Board board, Move move)
        {
            // A knight can move to any position in an L-shape from its current position as long
            // as long as no allied piece is on that tile.

            bool IsValidDestination((ChessFile f, int r) dest) =>
                FreeSpaceAt(board, dest.f, dest.r) || OpposingPieceAt(board, dest.f, dest.r);

            var (file, rank) = move.From;
            var (newFile, newRank) = move.To;

            var possibleDestinations = new[]
            {
                (file + 1, rank + 2),
                (file + 2, rank + 1),
                (file + 2, rank - 1),
                (file + 1, rank - 2),
                (file - 1, rank - 2),
                (file - 2, rank - 1),
                (file - 2, rank + 1),
                (file - 1, rank + 2),
            };

            if (!possibleDestinations.Where(IsValidDestination).Contains((newFile, newRank)))
            {
                throw new InvalidMoveException($"Cannot move to {move.To}");
            }
            
            return board.RepositionPiece(move.From, move.To);
        }
    }
}