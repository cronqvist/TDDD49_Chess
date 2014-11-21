using System.ComponentModel;

namespace Chess.Model
{
    public enum SquareBackground
    {
        White,
        Black,
        Move,
        Attacked
    }

    public class Square : INotifyPropertyChanged
    {
        #region property changed

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(p));
        }

        #endregion property changed

        private SquareBackground _background;
        private Piece _piece;

        public Square(SquareBackground bg)
        {
            Background = bg;

            OriginalBackground = bg;
        }

        public Piece Piece
        {
            get { return _piece; }
            set
            {
                _piece = value;
                OnPropertyChanged("Piece");
            }
        }

        public SquareBackground Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged("Background");
            }
        }

        public SquareBackground OriginalBackground { get; private set; }

        public void ResetBackground()
        {
            Background = OriginalBackground;
        }
    }
}