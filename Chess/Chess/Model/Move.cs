namespace Chess.Model
{
    public class Move
    {
        public Move(PiecePosition pos, MoveType type)
        {

            Position = pos;
            Type = type;
        }

        public PiecePosition Position { get; private set; }
        public MoveType Type { get; private set; }
    }
}