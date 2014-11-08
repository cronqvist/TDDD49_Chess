using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public enum MoveType
    {
        Move,
        Attack,
        PromoteQueen,
        PromoteKnight,
        PromoteRook,
        PromoteBishop
    }
    public class Move
    {
        public PiecePosition Position { get; private set; }
        public MoveType Type { get; private set; }

       public Move(PiecePosition pos, MoveType type)
        {
            Position = pos;
            Type = type;
        }
    }
}
