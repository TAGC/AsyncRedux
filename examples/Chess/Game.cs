using System.Threading.Tasks;
using AsyncRedux;

namespace Chess
{
    internal class Game
    {
        private readonly IStore<GameState> _store;

        public Game()
        {
            var initialState = new GameState(Board.StartingConfiguration, Player.White, null);

            _store = StoreSetup.CreateStore<GameState>()
                .FromReducer(ApplyMove)
                .WithInitialState(initialState)
                .Build();
        }

        public Board Board => _store.State.Board;

        public bool Complete => _store.State.Winner.HasValue;

        public Player? CurrentPlayer => _store.State.CurrentPlayer;

        public Player? Winner => _store.State.Winner;

        public Task PlayMove(Move move) => _store.Dispatch(move);

        private static GameState ApplyMove(GameState state, object action)
        {
            var move = (Move)action;
            var piece = state.Board[move.From];

            if (piece == null)
            {
                throw new InvalidMoveException($"No piece at {move.From}");
            }

            if (piece.Owner != state.CurrentPlayer)
            {
                throw new InvalidMoveException($"{state.CurrentPlayer} cannot move {piece.Symbol} at {move.From}");
            }

            var nextBoard = piece.ApplyMove(state.Board, move);

            if (CheckGameOver(nextBoard, out var winner))
            {
                return new GameState(nextBoard, null, winner);
            }

            var nextPlayer = state.CurrentPlayer == Player.White ? Player.Black : Player.White;

            return new GameState(nextBoard, nextPlayer, null);
        }

        private static bool CheckGameOver(Board board, out Player? winner)
        {
            // TODO.
            winner = null;
            return false;
        }
    }
}