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

        public abstract List<Movement> GetAvailableMoves();
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

        public override List<Movement> GetAvailableMoves()
        {
            List<Movement> moves = new List<Movement>();

            int forward;
            if (Color == Player.White)
            {
                forward = 1;
            }
            else
            {
                forward = -1;
            }

            moves.Add(new MovementPosition(new PiecePosition(Position.X, Position.Y + forward)));

            moves.Add(new MovementAttack(new PiecePosition(Position.X + 1, Position.Y + forward)));
            moves.Add(new MovementAttack(new PiecePosition(Position.X - 1, Position.Y + forward)));

            if (Position.Equals(startPosition))
            {
                moves.Add(new MovementPosition(new PiecePosition(Position.X, Position.Y + forward * 2)));
            }

            return moves;
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

        public override List<Movement> GetAvailableMoves()
        {
            List<Movement> moves = new List<Movement>();

            moves.Add(new MovementNormal(new PiecePosition(Position.X - 1, Position.Y)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X - 1, Position.Y - 1)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X - 1, Position.Y + 1)));

            moves.Add(new MovementNormal(new PiecePosition(Position.X + 1, Position.Y)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X + 1, Position.Y - 1)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X + 1, Position.Y + 1)));

            moves.Add(new MovementNormal(new PiecePosition(Position.X, Position.Y - 1)));
            moves.Add(new MovementNormal(new PiecePosition(Position.X, Position.Y + 1)));


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

        public override List<Movement> GetAvailableMoves()
        {
            throw new NotImplementedException();
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

        public override List<Movement> GetAvailableMoves()
        {
            throw new NotImplementedException();
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

        public override List<Movement> GetAvailableMoves()
        {
            throw new NotImplementedException();
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

        public override List<Movement> GetAvailableMoves()
        {
            throw new NotImplementedException();
        }
    }
}
