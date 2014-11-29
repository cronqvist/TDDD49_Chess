namespace Chess.Model
{
    public struct PiecePosition
    {
        public int X;
        public int Y;

        public PiecePosition(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(PiecePosition lhs, PiecePosition rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(PiecePosition lhs, PiecePosition rhs)
        {
            return !(lhs == rhs);
        } 

        public bool Equals(PiecePosition obj)
        {
            return X == obj.X && Y == obj.Y;
        }

        public override string ToString()
        {
            return "X: " + X + ", Y: " + Y;
        }
    }
}