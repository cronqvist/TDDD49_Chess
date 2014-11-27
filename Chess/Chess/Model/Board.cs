﻿using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Chess.Model
{
    public class GameBoard
    {
        private readonly Square[,] _board;
        public King BlackKing { get; private set; }
        public King WhiteKing { get; private set; }

        public List<Piece> BlackPieces { get; private set; }
        public List<Piece> WhitePieces { get; private set; }

        public List<Knight> WhiteKnights { get; private set; }
        public List<Knight> BlackKnights { get; private set; }

        public GameBoard()
        {
            _board = new Square[8, 8];
       

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((j - i + 1)%2 == 0)
                        _board[i, j] = new Square(SquareBackground.White);
                    else
                        _board[i, j] = new Square(SquareBackground.Black);
                }
            }

            BlackPieces = new List<Piece>();
            WhitePieces = new List<Piece>();

            WhiteKnights = new List<Knight>();
            BlackKnights= new List<Knight>();

            BuildStartPieces();
        }

        public void BuildStartPieces()
        {
            WhitePieces = buildPieces(Player.White);
            BlackPieces = buildPieces(Player.Black);
        }

        public void SetBoard(List<Piece> pieces)
        {
            ClearBoard();

            foreach (var piece in pieces)
            {
                _board[piece.Position.X, piece.Position.Y].Piece = piece;

                if (piece.Color == Player.Black)
                {
                    BlackPieces.Add(piece);
                }
                else
                {
                    WhitePieces.Add(piece);
                }
            }
        }

        public void ClearBoard()
        {
            WhitePieces.Clear();
            BlackPieces.Clear();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _board[i, j].Piece = null;
                }
            }
        }


        public Square this[int x, int y]
        {
            get { return _board[x, y]; }
        }

        public Square[,] Board
        {
            get { return _board; }
        }

        public GameBoard(GameBoard other)
        {
           /* for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    _board[i, j] = new Square(other._board[i, j]);
                }
            }

            _board = other._board;
            BlackKnights = other.BlackKnights;
            BlackPieces = other.BlackPieces;
            BlackKing = other.BlackKing;

            WhiteKnights = other.WhiteKnights;
            WhitePieces = other.WhitePieces;
            WhiteKing = other.WhiteKing;*/
        }


        public void SetBackgroundAt(int x, int y, SquareBackground bg)
        {
            _board[x, y].Background = bg;
        }

        private List<Piece> buildPieces(Player c)
        {
            var ret = new List<Piece>();
            int pawnRow;
            int royalRow;

            if (c == Player.Black)
            {
                pawnRow = 6;
                royalRow = 7;
            }
            else
            {
                pawnRow = 1;
                royalRow = 0;
            }

            ret.Add(_board[0, royalRow].Piece = new Rook(c, new PiecePosition(0, royalRow)));
            ret.Add( _board[2, royalRow].Piece = new Bishop(c, new PiecePosition(2, royalRow)));
            ret.Add(_board[3, royalRow].Piece = new Queen(c, new PiecePosition(3, royalRow)));
            ret.Add(_board[5, royalRow].Piece = new Bishop(c, new PiecePosition(5, royalRow)));
            ret.Add(_board[7, royalRow].Piece = new Rook(c, new PiecePosition(7, royalRow)));

            King king = new King(c, new PiecePosition(4, royalRow));
            Knight k1 = new Knight(c, new PiecePosition(1, royalRow));
            Knight k2 = new Knight(c, new PiecePosition(6, royalRow));

            if(c == Player.Black) {
                this.BlackKing = king;
                BlackKnights.Add(k1);
                BlackKnights.Add(k2);

            } else {
                this.WhiteKing = king;
                WhiteKnights.Add(k1);
                WhiteKnights.Add(k2);
            }

            ret.Add(_board[4, royalRow].Piece = king);
            ret.Add(_board[1, royalRow].Piece = k1);
            ret.Add(_board[6, royalRow].Piece = k2);

            for (int i = 0; i < 8; i++)
            {
                ret.Add(_board[i, pawnRow].Piece = new Pawn(c, new PiecePosition(i, pawnRow)));
            }

            return ret;
        }

        public Piece GetPieceAt(int x, int y)
        {
            return _board[x, y].Piece;
        }

        public static bool IsInBoard(int x, int y)
        {
            return (x >= 0 && x < 8) && (y >= 0 && y < 8);
        }
    }
}
