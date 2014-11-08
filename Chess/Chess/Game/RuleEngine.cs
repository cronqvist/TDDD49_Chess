using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    public class RuleEngine
    {
        private GameBoard _board;

        public RuleEngine(GameBoard board)
        {
            _board = board;
        }

        public List<Move> GetAvailableMoves(Pawn piece)
        {

            List<Move> ret = new List<Move>();
            PiecePosition pos = piece.Position;

            int nr_steps = pos.Y == 1 || pos.Y == 6 ? 2 : 1;
            int dir = piece.Color == Player.Black ? -1 : 1;

            IEnumerable<int> steps = Enumerable.Range(0,nr_steps);

            foreach(int i in steps) {
                int row = pos.Y + i*dir;
                Square curSquare = _board.Board[pos.X, row];

                if (curSquare.Piece == null)
                    ret.Add(new Move(new PiecePosition(pos.X, pos.Y), MoveType.Move));
            }

            int attackY = pos.Y + dir;
            int attackX1 = pos.X - 1;
            int attackX2 = pos.X + 1;

            if (GameBoard.IsInBoard(attackX1, attackY))
            {
                Piece other = _board.GetPieceAt(attackX1, attackY);
                if (other != null && other.Color != piece.Color) 
                {
                    ret.Add(new Move(new PiecePosition(pos.X, pos.Y), MoveType.Attack));
                }
            }

            if (GameBoard.IsInBoard(attackX2, attackY))
            {
                Piece other = _board.GetPieceAt(attackX2, attackY);
                if (other != null && other.Color != piece.Color)
                {
                    ret.Add(new Move(new PiecePosition(pos.X, pos.Y), MoveType.Attack));
                }
            }


            return ret;
        }
    }
}