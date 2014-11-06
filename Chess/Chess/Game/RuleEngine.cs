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
        private Square[][] board;

        public RuleEngine(Square[][] board)
        {
            this.board = board;
        }

        public List<Move> GetAvailableMoves(Piece piece)
        {
            List<Move> moves = new List<Move>();
            
            // piece.hämta movementvektorer typ
            List<PiecePosition> positions = piece.GetAvailableMoves();

            // beräkna giltliga drag 
            foreach (var pos in positions)
            {
                if (isValid(pos))
                {
                    moves.Add(new Move(pos, MoveType.Move));
                }
            }

            return moves;
        }

        private Boolean isInside(PiecePosition pos)
        {
            return (pos.X >= 0 && pos.X <= 7 && pos.Y >= 0 && pos.Y <= 7) ;
        }

        private Boolean isValid(PiecePosition pos)
        {
            return isInside(pos);
        }
    }
}
