using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public enum Player
    {
        White,
        Black
    }

    public struct PiecePosition
    {
        public int X, Y;

        public PiecePosition(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public bool Equals(PiecePosition obj)
        {
            return (this.X == obj.X && this.Y == obj.Y);
        }

        public override string ToString()
        {
            return "X: " + X + ", Y: " + Y;
        }
    }

    public abstract class Piece
    {
        public Player Color { get; private set; }

        public PiecePosition Position { get; set; }

        public String Filename { get; protected set; }

        public Piece(Player color, PiecePosition pos)
        {
            Color = color;
            Position = pos;
        }

        protected List<Move> GetMovesInLine(GameBoard board, int dirX, int dirY, int steps=8)
        {
            List<Move> ret = new List<Move>();
            PiecePosition curPos = new PiecePosition(Position.X + dirX, Position.Y + dirY);

            for (int i = 0; i < steps; ++i)
            {
               if(!GameBoard.IsInBoard(curPos.X, curPos.Y))
                    break;

                Square sq = board[curPos.X, curPos.Y];
                if (sq.Piece == null)
                {
                    ret.Add(new Move(curPos, MoveType.Move));
    }
                else if (sq.Piece.Color != Color)
                {
                    ret.Add(new Move(curPos, MoveType.Attack));
                    break;
                }
                else
                {
                    break;
                }

                curPos.X += dirX;
                curPos.Y += dirY;
            }

  

            return ret;
        }

        public abstract List<Move> GetAvailableMoves(GameBoard board);
    }

    public class Pawn : Piece
    {
        private PiecePosition startPosition;

        public Pawn(Player color, PiecePosition pos) 
            : base(color, pos)
        {
            startPosition = pos;

            if (color == Player.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wP.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bP.png";
            }
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            List<Move> ret = new List<Move>();
            PiecePosition pos = Position;

            int nr_steps = pos.Y == 1 || pos.Y == 6 ? 2 : 1;
            int dir = Color == Player.Black ? -1 : 1;

            IEnumerable<int> steps = Enumerable.Range(1,nr_steps);

            foreach(int i in steps) {
                int row = pos.Y + i*dir;
                Square curSquare = board.Board[pos.X, row];

                if (!GameBoard.IsInBoard(pos.X, row))
                    break;

                if (curSquare.Piece == null)
                {
                    ret.Add(new Move(new PiecePosition(pos.X, row), MoveType.Move));
                }
                else
                {
                    break;
                }
            }

            int attackY = pos.Y + dir;
            int attackX1 = pos.X - 1;
            int attackX2 = pos.X + 1;

            if (GameBoard.IsInBoard(attackX1, attackY))
            {
                Piece other = board.GetPieceAt(attackX1, attackY);
                if (other != null && other.Color != Color) 
                {
                    ret.Add(new Move(new PiecePosition(attackX1, attackY), MoveType.Attack));
            }
            }

            if (GameBoard.IsInBoard(attackX2, attackY))
            {
                Piece other = board.GetPieceAt(attackX2, attackY);
                if (other != null && other.Color != Color)
                {
                    ret.Add(new Move(new PiecePosition(attackX2, attackY), MoveType.Attack));
                }
            }

            return ret;
        }
    }

    public class King : Piece
    {
        public King(Player color, PiecePosition pos)
            : base(color, pos)
        {
            if (color == Player.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wK.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bK.png";
            }
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
           /* List<Movement> moves = new List<Movement>();

            moves.Add(new MovementNormal(new PiecePosition(Position.X - 1, Position.Y)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X - 1, Position.Y - 1)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X - 1, Position.Y + 1)));

            moves.Add(new MovementNormal(new PiecePosition(Position.X + 1, Position.Y)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X + 1, Position.Y - 1)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X + 1, Position.Y + 1)));

            moves.Add(new MovementNormal(new PiecePosition(Position.X, Position.Y - 1)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X, Position.Y + 1)));*/

            List<Move> moves = new List<Move>();

            moves.AddRange(GetMovesInLine(board, 1, 0, 1));
            moves.AddRange(GetMovesInLine(board, -1, 0, 1));
            moves.AddRange(GetMovesInLine(board, 0, 1, 1));
            moves.AddRange(GetMovesInLine(board, 0, -1, 1));
            moves.AddRange(GetMovesInLine(board, 1, 1, 1));
            moves.AddRange(GetMovesInLine(board, 1, -1, 1));
            moves.AddRange(GetMovesInLine(board, -1, 1, 1));
            moves.AddRange(GetMovesInLine(board, -1, -1, 1));

            return moves;
        }
    }

    public class Queen : Piece
    {
        public Queen(Player color, PiecePosition pos)
            : base(color, pos)
        {
            if (color == Player.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wQ.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bQ.png";
            }
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            List<Move> moves = new List<Move>();

            moves.AddRange(GetMovesInLine(board, 1, 0));
            moves.AddRange(GetMovesInLine(board, -1, 0));
            moves.AddRange(GetMovesInLine(board, 0, 1));
            moves.AddRange(GetMovesInLine(board, 0, -1));
            moves.AddRange(GetMovesInLine(board, 1, 1));
            moves.AddRange(GetMovesInLine(board, 1, -1));
            moves.AddRange(GetMovesInLine(board, -1, 1));
            moves.AddRange(GetMovesInLine(board, -1, -1));

            return moves;
        }
    }

    public class Rook : Piece
    {
        public Rook(Player color, PiecePosition pos)
            : base(color, pos)
        {
            if (color == Player.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wR.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bR.png";
            }
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            List<Move> moves = new List<Move>();

            moves.AddRange(GetMovesInLine(board, 1, 0));
            moves.AddRange(GetMovesInLine(board, -1, 0));
            moves.AddRange(GetMovesInLine(board, 0, 1));
            moves.AddRange(GetMovesInLine(board, 0, -1));
  
            return moves;
        }
    }

    public class Knight : Piece
    {
        public Knight(Player color, PiecePosition pos)
            : base(color, pos)
        {
            if (color == Player.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wN.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bN.png";
            }
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            List<Move> ret = new List<Move>();
            List<PiecePosition> posList = new List<PiecePosition>();
            PiecePosition origin = Position;


            posList.Add(new PiecePosition(origin.X + 2, origin.Y + 1));
            posList.Add(new PiecePosition(origin.X + 2, origin.Y - 1));

            posList.Add(new PiecePosition(origin.X - 2, origin.Y + 1));
            posList.Add(new PiecePosition(origin.X - 2, origin.Y - 1));

            posList.Add(new PiecePosition(origin.X + 1, origin.Y + 2));
            posList.Add(new PiecePosition(origin.X + 1, origin.Y - 2));

            posList.Add(new PiecePosition(origin.X - 1, origin.Y + 2));
            posList.Add(new PiecePosition(origin.X - 1, origin.Y - 2));

            foreach (var p in posList)
        {
                if (!GameBoard.IsInBoard(p.X, p.Y))
                    continue;

                Square sq = board[p.X, p.Y];
                if(sq.Piece == null) {
                    ret.Add(new Move(p, MoveType.Move));

                } else if(sq.Piece.Color != Color) {
                    ret.Add(new Move(p, MoveType.Attack));
                }
            }

            return ret;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(Player color, PiecePosition pos)
            : base(color, pos)
        {
            if (color == Player.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wB.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bB.png";
            }
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            List<Move> moves = new List<Move>();

            moves.AddRange(GetMovesInLine(board, 1, 1));
            moves.AddRange(GetMovesInLine(board, 1, -1));
            moves.AddRange(GetMovesInLine(board, -1, 1));
            moves.AddRange(GetMovesInLine(board, -1, -1));

            return moves;
        }
    }
}
