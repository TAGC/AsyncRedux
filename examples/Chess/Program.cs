using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Chess.Position;

namespace Chess
{
    internal static class Program
    {
        public static async Task Main()
        {
            var game = new Game();

            Console.WriteLine("Starting a new chess game.\n");
            Console.WriteLine(game.Board + "\n");
            Console.WriteLine("Specify moves in algebraic notation e.g. B4 to D5");
            Console.WriteLine("White goes first.\n");

            while (!game.Complete)
            {
                var move = QueryPlayerForMove(game.CurrentPlayer.Value);

                try
                {
                    await game.PlayMove(move);
                    Console.WriteLine("\n" + game.Board);
                }
                catch (InvalidMoveException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine($"Game over! {game.Winner.Value} won!");
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }

        private static Move QueryPlayerForMove(Player player)
        {
            while (true)
            {
                Console.Write($"{player}> ");

                if (TryParseMove(Console.ReadLine(), player, out var move))
                {
                    return move.Value;
                }

                Console.WriteLine("Not a valid move");
            }
        }

        private static bool TryParseMove(string input, Player player, out Move? move)
        {
            move = null;

            var match = Regex.Match(input, @"([a-hA-H])([1-8])\s+to\s+([a-hA-H])([1-8])");

            if (!match.Success)
            {
                return false;
            }

            if (!Position.TryCreate(
                file: Enum.Parse<ChessFile>(match.Groups[1].Value, true),
                rank: int.Parse(match.Groups[2].Value),
                position: out var from))
            {
                return false;
            }

            if (!Position.TryCreate(
                file: Enum.Parse<ChessFile>(match.Groups[3].Value, true),
                rank: int.Parse(match.Groups[4].Value),
                position: out var to))
            {
                return false;
            }

            move = new Move(player, from.Value, to.Value);
            return true;
        }
    }
}