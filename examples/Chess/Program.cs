using System;
using System.Text;

namespace Chess
{
    internal static class Program
    {
        public static void Main()
        {
            var game = new Game();

            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("♔");
            Console.WriteLine("Started new game");
            Console.WriteLine(game.Board);

            Console.Read();
        }
    }
}