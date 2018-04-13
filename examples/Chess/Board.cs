using System.Text;
using Chess.Pieces;
using static Chess.Position.ChessFile;

namespace Chess
{
    internal class Board
    {
        private readonly IPiece[,] _pieces;

        private Board()
        {
            _pieces = new IPiece[8, 8];
        }

        private Board(IPiece[,] pieces)
        {
            _pieces = pieces;
        }

        public static Board StartingConfiguration
        {
            get
            {
                var board = new Board();

                foreach (var player in new[] { Player.White, Player.Black })
                {
                    var white = player == Player.White;
                    var rank = white ? 1 : 8;
                    var factory = white ? Factory.White : Factory.Black;

                    board[A, rank] = factory.Rook;
                    board[B, rank] = factory.Knight;
                    board[C, rank] = factory.Bishop;
                    board[D, rank] = factory.Queen;
                    board[E, rank] = factory.King;
                    board[F, rank] = factory.Bishop;
                    board[G, rank] = factory.Knight;
                    board[H, rank] = factory.Rook;

                    for (var file = A; file <= H; file++)
                    {
                        board[file, white ? 2 : 7] = factory.Pawn;
                    }
                }

                return board;
            }
        }

        public IPiece this[Position position]
        {
            get => this[position.File, position.Rank];
            private set => this[position.File, position.Rank] = value;
        }

        public IPiece this[Position.ChessFile file, int rank]
        {
            get => _pieces[(int)file - 1, 8 - rank];
            private set => _pieces[(int)file - 1, 8 - rank] = value;
        }

        public Board RepositionPiece(Position from, Position to)
        {
            var copy = Copy();

            // Overwrites piece at position, if any.
            copy[to] = copy[from];
            copy[from] = null;

            return copy;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            //const char whiteSquare = '◻';
            //const char blackSquare = '◼';

            char SymbolAtPosition(Position.ChessFile file, int rank)
            {
                var piece = this[file, rank];

                if (piece != null)
                {
                    return piece.Symbol;
                }

                //return ((int)file + rank) % 2 != 0 ? whiteSquare : blackSquare;
                return '▢';
            }

            var stringBuilder = new StringBuilder();

            for (var rank = 8; rank >= 1; rank--)
            {
                stringBuilder.Append(rank);

                for (var file = A; file <= H; file++)
                {
                    stringBuilder.Append(" " + SymbolAtPosition(file, rank));
                }

                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine("  A B C D E F G H");

            return stringBuilder.ToString();
        }

        private Board Copy() => new Board(_pieces);
    }
}