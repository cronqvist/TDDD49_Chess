using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Chess.Model
{
    public enum Backgrounds
    {
        Original,
        Move,
        Attacked
    }

    public struct SquarePosition
    {
        int X, Y;

        public SquarePosition(int x, int y) : this()
        {
            X = x;
            Y = y;
        }
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

        private Brush originalBackground;
        private Brush attackedBackground;
        private Brush moveBackground;

        private Brush _background;
        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged("Background");
            }
        }

        public SquarePosition Position;

        public Square(SquarePosition pos, Brush background)
        {
            Position = pos;
            Background = background;

            originalBackground = background;
            attackedBackground = new SolidColorBrush(Colors.Red);
            moveBackground = new SolidColorBrush(Colors.LightGreen);
        }

        public void SetBackground(Backgrounds bg)
        {
            if (bg == Backgrounds.Original)
            {
                Background = originalBackground;
            }
            else if (bg == Backgrounds.Move)
            {
                Background = moveBackground;
            }
            else if (bg == Backgrounds.Attacked)
            {
                Background = attackedBackground;
            }
        }
    }
}
