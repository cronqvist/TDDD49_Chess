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
            PiecePosition orgPos = piece.Position;

            //Check for "self check" 
            /*foreach (var move in potentialMoves)
            {
                GameBoard newBoard = new GameBoard(_board);
                King mKing = piece.Color == Player.White ? newBoard.WhiteKing : newBoard.BlackKing;

                Square startSquare = newBoard.Board[orgPos.X, orgPos.Y];
                Square endSquare = newBoard.Board[move.Position.X, move.Position.Y];

                endSquare.Piece = startSquare.Piece;
                startSquare.Piece = null;

                if (!IsThreatened(mKing))
                    ret.Add(move); 
            }*/

            ret = potentialMoves;

            return ret;
        }

        private bool IsThreatened(Piece p)
        {
            bool dirThreat = ThreatInDir(p, 0, 1)   ||
                             ThreatInDir(p, 0, -1)  ||
                             ThreatInDir(p, 1, 0)   ||
                             ThreatInDir(p, -1, 0)  ||
                             ThreatInDir(p, 1, 1)   ||
                             ThreatInDir(p, 1, -1)  ||
                             ThreatInDir(p, -1, 1)  ||
                             ThreatInDir(p, -1, -1);

            if (dirThreat)
                return dirThreat;

            return KnightThreat(p);
        }

        private bool KnightThreat(Piece p)
        {
            List<Knight> knights = p.Color == Player.White ? _board.BlackKnights : _board.WhiteKnights;

            foreach (var knight in knights)
            {
                List<Move> moves = knight.GetAvailableMoves(_board);
                foreach (var move in moves)
                {
                    if (move.Type == MoveType.Attack && move.Position == p.Position)
                        return true;
                }
            }

            return false;
        }

        private bool ThreatInDir(Piece p, int xDir, int yDir)
        {
            int nr_steps = 8;
            PiecePosition pos = p.Position;
            Player myColor = p.Color;
            IEnumerable<int> steps = Enumerable.Range(1,nr_steps);
            foreach (int step in steps)
            {
                PiecePosition nPos = new PiecePosition(pos.X + step * xDir, pos.Y + step * yDir);
                
                if(!GameBoard.IsInBoard(nPos.X, nPos.Y)) 
                    break;
                
                Piece otherPiece = _board.GetPieceAt(nPos.X, nPos.Y);

                if (otherPiece != null) 
                {

                    if (otherPiece.Color == myColor)
                        return false;

                    List<Move> moves = otherPiece.GetAvailableMoves(_board);
                    foreach (var move in moves)
                    {
                        if (move.Type == MoveType.Attack && move.Position == pos)
                            return true;
                    }

                    return false;
                }
            }

            return false;
        }

        public bool IsCheck(Player p) {

            King king = p == Player.White ? _board.WhiteKing : _board.BlackKing;
            return IsThreatened(king);
        }

        public bool IsCheckMate(Player p)
        {
           /* if (IsCheck(p))
            {
                List<Piece> pieces = p == Player.White ? _board.WhitePieces : _board.BlackPieces;
                foreach (var piece in pieces)
                {
                    List<Move> moves = piece.GetAvailableMoves(_board);
                    foreach (var move in moves)
                    {

                    }
                }
            }*/


            return false;
        }

    }
}
