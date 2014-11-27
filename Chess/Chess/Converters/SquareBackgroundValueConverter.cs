using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Chess.Model;

namespace Chess.Converters
{
    public class SquareBackgroundValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bg = (SquareBackground) value;
            var brush = new SolidColorBrush();

            switch (bg)
            {
                case SquareBackground.Attacked:
                    brush.Color = Colors.Red;
                    break;
                case SquareBackground.Black:
                    brush.Color = Colors.LightGray;
                    break;
                case SquareBackground.Move:
                    brush.Color = Colors.LightGreen;
                    break;
                default:
                    brush.Color = Colors.White;
                    break;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}