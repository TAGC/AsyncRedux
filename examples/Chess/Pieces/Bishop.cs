﻿using System;

namespace Chess.Pieces
{
    internal class Bishop : Piece
    {
        /// <inheritdoc />
        public Bishop(Player owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override char BlackSymbol { get; } = '♝';

        /// <inheritdoc />
        protected override char WhiteSymbol { get; } = '♗';

        /// <inheritdoc />
        public override Board ApplyMove(Board board, Move move) => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool CanApplyMove(Board currentBoard, Move move) => throw new NotImplementedException();
    }
}