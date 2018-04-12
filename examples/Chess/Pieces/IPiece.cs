namespace Chess.Pieces
{
    internal interface IPiece
    {
        Player Owner { get; }

        char Symbol { get; }

        Board ApplyMove(Board board, Move move);
    }
}