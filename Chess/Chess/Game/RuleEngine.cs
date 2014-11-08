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
            List<Movement> positions = piece.GetAvailableMoves();

            // beräkna giltliga drag 
            foreach (var pos in positions)
            {
                if (pos is MovementPosition)
                {
                    if (isValid(pos.Position) && isEmpty(pos.Position))
                    {
                        moves.Add(new Move(pos.Position, MoveType.Move));
                    }
                }
                else if (pos is MovementNormal)
                {
                    if (isInside(pos.Position))
                    {
                        if (isEmpty(pos.Position))
                        {
                            moves.Add(new Move(pos.Position, MoveType.Move));
                        }
                        else if (isEnemy(piece, pos.Position))
                        {
                            moves.Add(new Move(pos.Position, MoveType.Attack));
                        }
                    }
                }
                else if (pos is MovementVector)
                {

                }
                else if (pos is MovementAttack)
                {
                    if (isValid(pos.Position) && isEnemy(piece, pos.Position))
                    {
                        moves.Add(new Move(pos.Position, MoveType.Attack));
                    }
                }
            }

            return moves;
        }

        private Boolean isInside(PiecePosition pos)
        {
            return (pos.X >= 0 && pos.X <= 7 && pos.Y >= 0 && pos.Y <= 7);
        }

        private Boolean isEmpty(PiecePosition pos)
        {
            return (board[pos.X][pos.Y].Piece == null);
        }

        private Boolean isValid(PiecePosition pos)
        {
            return isInside(pos);
        }

        private Boolean isEnemy(Piece piece, PiecePosition pos)
        {
            if (board[pos.X][pos.Y].Piece != null)
            {
                return (board[pos.X][pos.Y].Piece.Color != piece.Color);
            }

            return false;
        }
    }
}
