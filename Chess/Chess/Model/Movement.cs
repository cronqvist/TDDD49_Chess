using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Model
{
    public abstract class Movement
    {
        public PiecePosition Position { get; private set; }

        public Movement(PiecePosition pos)
        {
            Position = pos;
        }
    }

    public class MovementVector : Movement
    {
        public MovementVector(PiecePosition pos)
            : base(pos)
        {}
    }

    public class MovementPosition : Movement
    {
        public MovementPosition(PiecePosition pos)
            : base(pos)
        { }
    }

    public class MovementAttack : Movement
    {
        public MovementAttack(PiecePosition pos)
            : base(pos)
        { }
    }
}
