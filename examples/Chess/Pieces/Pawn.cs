using System;
using System.Collections.Generic;
using static Chess.Position;
using static Chess.Position.ChessFile;

namespace Chess.Pieces
{
    internal class Pawn : Piece
    {
        public Pawn(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override char BlackSymbol { get; } = '♟';

        /// <inheritdoc />
        protected override char WhiteSymbol { get; } = '♙';

        /// <inheritdoc />
        public override Board ApplyMove(Board board, Move move)
        {
            var (file, rank) = move.From;
            var (newFile, newRank) = move.To;
            var possibleNextRank = Owner == Player.White ? rank + 1 : rank - 1;

            // Pawn can move if:
            //  1) the space ahead of it is free
            //  2) there is an opposing piece along one diagonal ahead of it
            //  3) there is an opposing piece along another diagonal ahead of it

            switch ((newFile, newRank))
            {
                case var p1 when p1.Equals((file, possibleNextRank)) && FreeSpaceAt(board, newFile, newRank):
                case var p2 when p2.Equals((file - 1, possibleNextRank)) && OpposingPieceAt(board, newFile, newRank):
                case var p3 when p3.Equals((file + 1, possibleNextRank)) && OpposingPieceAt(board, newFile, newRank):
                    return board.RepositionPiece(move.From, move.To);

                default:
                    throw new InvalidMoveException($"Cannot move to {move.To}");
            }
        }
    }
}