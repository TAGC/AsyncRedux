using System;
using System.Linq;
using static Chess.Position;

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
        public override Board ApplyMove(Board board, Move move)
        {
            // A king can move one tile in any direction as long as no allied piece is on that tile.

            bool IsValidDestination((ChessFile f, int r) dest) =>
                FreeSpaceAt(board, dest.f, dest.r) || OpposingPieceAt(board, dest.f, dest.r);

            var (file, rank) = move.From;
            var (newFile, newRank) = move.To;

            var possibleDestinations = from fileDelta in new[] { -1, 0, 1 }
                                       from rankDelta in new[] { -1, 0, 1 }
                                       where fileDelta != 0 || rankDelta != 0
                                       select (file + fileDelta, rank + rankDelta);

            if (!possibleDestinations.Where(IsValidDestination).Contains((newFile, newRank)))
            {
                throw new InvalidMoveException($"Cannot move to {move.To}");
            }
            
            return board.RepositionPiece(move.From, move.To);
        }
    }
}