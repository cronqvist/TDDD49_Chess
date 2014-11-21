using System.Collections.Generic;

namespace Chess.Model
{
    public class GameBoard
    {
        private readonly Square[,] _board;

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

            WhitePieces = BuildPieces(Player.White);
            BlackPieces = BuildPieces(Player.Black);
        }

        /*private List<Piece> _whitePieces;
        private List<Piece> _blackPieces;*/

        public List<Piece> BlackPieces { get; private set; }
        public List<Piece> WhitePieces { get; private set; }

        public Square this[int x, int y]
        {
            get { return _board[x, y]; }
        }

        public Square[,] Board
        {
            get { return _board; }
        }


        public void SetBackgroundAt(int x, int y, SquareBackground bg)
        {
            _board[x, y].Background = bg;
        }

        private List<Piece> BuildPieces(Player c)
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
            ret.Add(_board[1, royalRow].Piece = new Knight(c, new PiecePosition(1, royalRow)));
            ret.Add(_board[2, royalRow].Piece = new Bishop(c, new PiecePosition(2, royalRow)));
            ret.Add(_board[3, royalRow].Piece = new Queen(c, new PiecePosition(3, royalRow)));
            ret.Add(_board[4, royalRow].Piece = new King(c, new PiecePosition(4, royalRow)));
            ret.Add(_board[5, royalRow].Piece = new Bishop(c, new PiecePosition(5, royalRow)));
            ret.Add(_board[6, royalRow].Piece = new Knight(c, new PiecePosition(6, royalRow)));
            ret.Add(_board[7, royalRow].Piece = new Rook(c, new PiecePosition(7, royalRow)));
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