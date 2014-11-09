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

        public List<Move> GetAvailableMoves(Piece piece)
        {
            List<Move> ret = new List<Move>();

            List<Move> potentialMoves = piece.GetAvailableMoves(_board);

            //TODO: Check for "self check" 

            ret = potentialMoves;

            return ret;

        }

        public bool IsCheck(Player p) {

            List<Piece> pieces = p == Player.White ? _board.WhitePieces : _board.BlackPieces;

            foreach (var piece in pieces)
            {
                List<Move> moves = piece.GetAvailableMoves(_board);
                foreach (var move in moves)
                {
                    if (move.Type == MoveType.Attack)
                        if(_board.GetPieceAt(move.Position.X, move.Position.Y).IsKing())
                            return true;
                }
            }

            return false;
        }

        public bool IsCheckMate(Player p)
        {


            return false;
        }

    }
}
