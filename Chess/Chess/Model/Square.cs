using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

        private Piece _piece;
        public Piece Piece
        {
            get { return _piece; }
            set
            {
                _piece = value;
                OnPropertyChanged("Piece");
            }
        }

        private SquareBackground _background;
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

        public Square(SquareBackground bg)
        {
            Background = bg;

            OriginalBackground = bg;
        }

        public void ResetBackground()
        {
            Background = OriginalBackground;
        }
    }
}
