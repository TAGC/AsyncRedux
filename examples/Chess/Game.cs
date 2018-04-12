using System;
using System.Threading.Tasks;
using AsyncRedux;

namespace Chess
{
    internal class Game
    {
        private readonly IStore<Board> _store;

        public Game()
        {
            _store = StoreSetup.CreateStore<Board>()
                .FromReducer(ApplyMove)
                .WithInitialState(Board.StartingConfiguration)
                .Build();
        }

        public Board Board => _store.State;

        public Task PlayMove(Move move) => _store.Dispatch(move);

        private static Board ApplyMove(Board board, object action)
        {
            var move = (Move)action;
            var piece = board[move.From] ?? throw new ArgumentException($"No piece at {move.From}");

            return piece.ApplyMove(board, move);
        }
    }
}