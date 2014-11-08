using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public class GameBoard
    {
        private Square[,] _board;

        public GameBoard()
        {
            _board = new Square[8,8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((j - i + 1) % 2 == 0)
                        _board[i,j] = new Square(SquareBackground.White);
                    else
                        _board[i,j] = new Square(SquareBackground.Black);
                }
            }

            BuildPieces(Player.Black);
            BuildPieces(Player.White);
        }

        public void SetBackgroundAt(int x, int y, SquareBackground bg) {
            _board[x, y].Background = bg;
        }

        private void BuildPieces(Player c) {
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

            _board[0, royalRow].Piece = new Rook(c, new PiecePosition(0, royalRow));
            _board[1, royalRow].Piece = new Knight(c, new PiecePosition(1, royalRow));
            _board[2, royalRow].Piece = new Bishop(c, new PiecePosition(2, royalRow));
            _board[3, royalRow].Piece = new Queen(c, new PiecePosition(3, royalRow));
            _board[4, royalRow].Piece = new King(c, new PiecePosition(4, royalRow));
            _board[5, royalRow].Piece = new Bishop(c, new PiecePosition(5, royalRow));
            _board[6, royalRow].Piece = new Knight(c, new PiecePosition(6, royalRow));
            _board[7, royalRow].Piece = new Rook(c, new PiecePosition(7, royalRow));
            for (int i = 0; i < 8; i++)
            {
                _board[i, pawnRow].Piece = new Pawn(c, new PiecePosition(i, pawnRow));
            }
        }

        public Piece GetPieceAt(int x, int y)
        {
            return _board[x, y].Piece;
        }

        public static bool IsInBoard(int x, int y)
        {
            return (x >= 0 && x < 8) && (y >= 0 && y < 8);
        }

        public Square this[int x, int y]
        {
            get
            {
                return _board[x, y];
            }
        }

        public Square[,] Board
        {
            get { return _board; }
        }
    }
}
