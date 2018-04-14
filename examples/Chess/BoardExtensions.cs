using System;
using System.Collections.Generic;
using Chess.Pieces;
using static Chess.Position;

namespace Chess
{
    internal static class BoardExtensions
    {
        public static IEnumerable<Position> FindPiecesByType<T>(this Board board, Player owner)
            where T : IPiece
        {
            for (var file = MinFile; file <= MaxFile; file++)
            {
                for (var rank = MinRank; rank <= MaxRank; rank++)
                {
                    if (board[file, rank] is T piece && piece.Owner == owner)
                    {
                        yield return new Position(file, rank);
                    }
                }
            }
        }
    }
}
