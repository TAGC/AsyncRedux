using System;
using static Chess.Position;

namespace Chess.Pieces
{
    internal class Rook : Piece
    {
        /// <inheritdoc />
        public Rook(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override char BlackSymbol { get; } = '♜';

        /// <inheritdoc />
        protected override char WhiteSymbol { get; } = '♖';

        /// <inheritdoc />
        public override Board ApplyMove(Board board, Move move)
        {
            // Rook can move if:
            //  1) the move is straight (vertical / horizontal) and ends at a free space with no pieces in-between
            //  2) the move is straight (vertical / horizontal) and ends on an opposing piece with no pieces in-between

            var (file, rank) = move.From;
            var (newFile, newRank) = move.To;
            
            var fileDelta = Math.Sign(newFile - file);
            var rankDelta = Math.Sign(newRank - rank);
            var horizontalSteps = Math.Abs(newFile - file);
            var verticalSteps = Math.Abs(newRank - rank);
            var vertical = verticalSteps > 0 && horizontalSteps == 0;
            var horizontal = verticalSteps == 0 && horizontalSteps > 0;
            var numSteps = vertical ? verticalSteps : horizontalSteps;

            if (!vertical && !horizontal)
            {
                throw new InvalidMoveException($"Cannot move to {move.To}");
            }

            for (var i = 1; i <= numSteps; i++)
            {
                var currentFile = file + fileDelta * i;
                var currentRank = rank + rankDelta * i;

                if (!CanStep(board, currentFile, currentRank, i == numSteps))
                {
                    throw new InvalidMoveException($"Cannot move to {move.To}");
                }
            }

            return board.RepositionPiece(move.From, move.To);
        }

        private bool CanStep(Board board, ChessFile file, int rank, bool lastStep)
        {
            if (FreeSpaceAt(board, file, rank)) return true;
            if (OpposingPieceAt(board, file, rank) && lastStep) return true;

            return false;
        }
    }
}