using System;

namespace Chess
{
    internal class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message)
            : base(message)
        {
        }

        public InvalidMoveException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}