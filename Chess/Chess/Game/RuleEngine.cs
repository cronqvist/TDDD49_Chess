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

            // beräkna giltliga drag 

            return moves;
        }

        private Boolean isInside(PiecePosition pos)
        {
            return true;
        }
    }
}
