using Chess.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Model
{
    public abstract class Piece
    {

        public int Value { get; protected set; }
            
        protected Piece(PlayerColor color, PiecePosition pos)
        {
            Color = color;
            Position = pos;

        }

        public PlayerColor Color { get; protected set; }
        public PiecePosition Position { get; set; }
        public String Filename { get; protected set; }

        public abstract Piece Clone();
        public virtual bool IsKing()
        {
            return false;
        }
        public virtual bool IsKnight()
        {
            return false;
        }

        protected List<Move> GetMovesInLine(GameBoard board, int dirX, int dirY, int steps = 8)
        {
            var ret = new List<Move>();
            var curPos = new PiecePosition(Position.X + dirX, Position.Y + dirY);

            for (int i = 0; i < steps; ++i)
            {
                if (!GameBoard.IsInBoard(curPos.X, curPos.Y))
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

        private bool hasMoved;

        public Pawn(PlayerColor color, PiecePosition pos, bool moved = false)
            : base(color, pos)
        {
            Value = 1;
            hasMoved = moved;

            if (color == PlayerColor.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wP.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bP.png";
            }
        }

  
        public override Piece Clone()
        {
            return new Pawn(Color, Position, hasMoved);
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            var ret = new List<Move>();
            PiecePosition pos = Position;

            int nrSteps = 1;

            if ((Color == PlayerColor.White && Position.Y == 1) || (Color == PlayerColor.Black && Position.Y == 6))
                nrSteps = 2;
       
            int dir = Color == PlayerColor.Black ? -1 : 1;

            IEnumerable<int> steps = Enumerable.Range(1, nrSteps);

            foreach (int i in steps)
            {
                int row = pos.Y + i*dir;

                if (!GameBoard.IsInBoard(pos.X, row))
                    break;
                
                Square curSquare = board.Board[pos.X, row];

                if (curSquare.Piece == null)
                {
                    PiecePosition newPos =  new PiecePosition(pos.X, row);
                    if (RuleEngine.IsPromotePos(newPos))
                    {
                        ret.Add(new Move(newPos, MoveType.PromoteQueen));
                    }
                    else 
                    {
                        ret.Add(new Move(newPos, MoveType.Move));
                    }
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
                Piece other = board[attackX1, attackY].Piece;
                if (other != null && other.Color != Color)
                {
                    PiecePosition newPos = new PiecePosition(attackX1, attackY);
                    MoveType t = RuleEngine.IsPromotePos(newPos) ? MoveType.PromoteQueen : MoveType.Attack;
                    ret.Add(new Move(newPos, t));
                }
            }

            if (GameBoard.IsInBoard(attackX2, attackY))
            {
                Piece other = board.GetPieceAt(attackX2, attackY);
                if (other != null && other.Color != Color)
                {
                    PiecePosition newPos = new PiecePosition(attackX2, attackY);
                    MoveType t = RuleEngine.IsPromotePos(newPos) ? MoveType.PromoteQueen : MoveType.Attack;
                    ret.Add(new Move(newPos, t));
                }
            }

            return ret;
        }
    }

    public class King : Piece
    {
        public King(PlayerColor color, PiecePosition pos)
            : base(color, pos)
        {

            Value = 0;

            if (color == PlayerColor.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wK.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bK.png";
            }
        }

        public override Piece Clone()
        {
            return new King(Color, Position);
        }

        public override bool IsKing()
        {
            return true;
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

            var moves = new List<Move>();

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
        public Queen(PlayerColor color, PiecePosition pos)
            : base(color, pos)
        {
            Value = 9;

            if (color == PlayerColor.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wQ.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bQ.png";
            }
        }

        public override Piece Clone()
        {
            return new Queen(Color, Position);
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            var moves = new List<Move>();

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
        public Rook(PlayerColor color, PiecePosition pos)
            : base(color, pos)
        {
            Value = 5;

            if (color == PlayerColor.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wR.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bR.png";
            }
        }

        public override Piece Clone()
        {
            return new Rook(Color, Position);
        }


        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            var moves = new List<Move>();

            moves.AddRange(GetMovesInLine(board, 1, 0));
            moves.AddRange(GetMovesInLine(board, -1, 0));
            moves.AddRange(GetMovesInLine(board, 0, 1));
            moves.AddRange(GetMovesInLine(board, 0, -1));

            return moves;
        }
    }

    public class Knight : Piece
    {


        public Knight(PlayerColor color, PiecePosition pos)
            : base(color, pos)
        {

            Value = 3;

            if (color == PlayerColor.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wN.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bN.png";
            }
        }

        public override Piece Clone()
        {
            return new Knight(Color, Position);
        }


        public override bool IsKnight()
        {
            return true;
        }

        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            var ret = new List<Move>();
            var posList = new List<PiecePosition>();
            PiecePosition origin = Position;


            posList.Add(new PiecePosition(origin.X + 2, origin.Y + 1));
            posList.Add(new PiecePosition(origin.X + 2, origin.Y - 1));

            posList.Add(new PiecePosition(origin.X - 2, origin.Y + 1));
            posList.Add(new PiecePosition(origin.X - 2, origin.Y - 1));

            posList.Add(new PiecePosition(origin.X + 1, origin.Y + 2));
            posList.Add(new PiecePosition(origin.X + 1, origin.Y - 2));

            posList.Add(new PiecePosition(origin.X - 1, origin.Y + 2));
            posList.Add(new PiecePosition(origin.X - 1, origin.Y - 2));

            foreach (PiecePosition p in posList)
            {
                if (!GameBoard.IsInBoard(p.X, p.Y))
                    continue;

                Square sq = board[p.X, p.Y];
                if (sq.Piece == null)
                {
                    ret.Add(new Move(p, MoveType.Move));
                }
                else if (sq.Piece.Color != Color)
                {
                    ret.Add(new Move(p, MoveType.Attack));
                }
            }

            return ret;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(PlayerColor color, PiecePosition pos)
            : base(color, pos)
        {
            Value = 3;

            if (color == PlayerColor.White)
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/wB.png";
            }
            else
            {
                Filename = "pack://application:,,,/Chess;component/Resources/pieces/bB.png";
            }
        }

        public override Piece Clone()
        {
            return new Bishop(Color, Position);
        }



        public override List<Move> GetAvailableMoves(GameBoard board)
        {
            var moves = new List<Move>();

            moves.AddRange(GetMovesInLine(board, 1, 1));
            moves.AddRange(GetMovesInLine(board, 1, -1));
            moves.AddRange(GetMovesInLine(board, -1, 1));
            moves.AddRange(GetMovesInLine(board, -1, -1));

            return moves;
        }
    }
}
