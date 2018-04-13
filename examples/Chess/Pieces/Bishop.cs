using System;
using static Chess.Position;

namespace Chess.Pieces
{
    internal class Bishop : Piece
    {
        /// <inheritdoc />
        public Bishop(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override char BlackSymbol { get; } = '♝';

        /// <inheritdoc />
        protected override char WhiteSymbol { get; } = '♗';

        /// <inheritdoc />
        public override Board ApplyMove(Board board, Move move)
        {
            // Bishop can move if:
            //  1) the move is diagonal and ends at a free space with no pieces in-between
            //  2) the move is diagonal and ends on an opposing piece with no pieces in-between

            var (file, rank) = move.From;
            var (newFile, newRank) = move.To;

            var fileDelta = Math.Sign(newFile - file);
            var rankDelta = Math.Sign(newRank - rank);
            var fileSteps = Math.Abs(newFile - file);
            var rankSteps = Math.Abs(newRank - rank);
            var diagonal = fileSteps == rankSteps;

            if (!diagonal || (fileDelta == 0) || (rankDelta == 0))
            {
                throw new InvalidMoveException($"Cannot move to {move.To}");
            }

            for (var i = 1; i <= fileSteps; i++)
            {
                var currentFile = file + (fileDelta * i);
                var currentRank = rank + (rankDelta * i);

                if (!CanStep(board, currentFile, currentRank, i == fileSteps))
                {
                    throw new InvalidMoveException($"Cannot move to {move.To}");
                }
            }

            return board.RepositionPiece(move.From, move.To);
        }

        private bool CanStep(Board board, ChessFile file, int rank, bool lastStep)
        {
            if (FreeSpaceAt(board, file, rank))
            {
                return true;
            }

            if (OpposingPieceAt(board, file, rank) && lastStep)
            {
                return true;
            }

            return false;
        }
    }
}