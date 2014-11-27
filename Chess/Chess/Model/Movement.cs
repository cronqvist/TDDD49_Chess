namespace Chess.Model
{
    public abstract class Movement
    {
        protected Movement(PiecePosition pos)
        {
            Position = pos;
        }

        public PiecePosition Position { get; private set; }
    }

    public class MovementNormal : Movement
    {
        public MovementNormal(PiecePosition pos)
            : base(pos)
        {
        }
    }

    public class MovementVector : Movement
    {
        public MovementVector(PiecePosition pos)
            : base(pos)
        {
        }
    }

    public class MovementPosition : Movement
    {
        public MovementPosition(PiecePosition pos)
            : base(pos)
        {
        }
    }

    public class MovementAttack : Movement
    {
        public MovementAttack(PiecePosition pos)
            : base(pos)
        {
        }
    }
}