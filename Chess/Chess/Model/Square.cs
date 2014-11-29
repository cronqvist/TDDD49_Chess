using Utilities;

namespace Chess.Model
{
    public class Square : NotificationObject
    {
        public Square(SquareBackground bg)
        {
            Background = bg;

            OriginalBackground = bg;
        }

        private Piece _piece;
        public Piece Piece
        {
            get { return _piece; }
            set
            {
                _piece = value;
                OnPropertyChanged();
            }
        }

        private SquareBackground _background;
        public SquareBackground Background
        {
            get { return _background; }
            set
            {
                if (value == _background) return;
                _background = value;
                OnPropertyChanged();
            }
        }

        public SquareBackground OriginalBackground { get; private set; }

        public void ResetBackground()
        {
            Background = OriginalBackground;
        }
    }
}
