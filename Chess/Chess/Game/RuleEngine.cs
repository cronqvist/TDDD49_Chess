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

        public List<PiecePosition> GetAvailableMoves(Piece piece)
        {
            List<PiecePosition> moves = new List<PiecePosition>();
            // piece.hämta movementvektorer typ

            // beräkna giltliga drag 

            return moves;
        }

        private Boolean isInside(PiecePosition pos)
        {

            return null;
        }
    }
}
