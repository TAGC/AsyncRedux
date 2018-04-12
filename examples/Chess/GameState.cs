using System;

namespace Chess
{
    internal class GameState
    {
        public GameState(Board board, Player? currentPlayer, Player? winner)
        {
            Board = board;
            CurrentPlayer = currentPlayer;
            Winner = winner;
        }

        public Board Board { get; }

        public Player? CurrentPlayer { get; }

        public Player? Winner { get; }
    }
}
