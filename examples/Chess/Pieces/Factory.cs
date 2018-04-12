namespace Chess.Pieces
{
    internal class Factory
    {
        private Factory(Player owner)
        {
            Bishop = new Bishop(owner);
            King = new King(owner);
            Knight = new Knight(owner);
            Pawn = new Pawn(owner);
            Queen = new Queen(owner);
            Rook = new Rook(owner);
        }

        public static Factory Black { get; } = new Factory(Player.Black);

        public static Factory White { get; } = new Factory(Player.White);

        public IPiece Bishop { get; }

        public IPiece King { get; }

        public IPiece Knight { get; }

        public IPiece Pawn { get; }

        public IPiece Queen { get; }

        public IPiece Rook { get; }
    }
}