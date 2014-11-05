using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Chess.Converters
{
    public class SquareBackgroundValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SquareBackground bg = (SquareBackground)value;
            SolidColorBrush brush = new SolidColorBrush();

            if (bg == SquareBackground.Attacked)
            {
                brush.Color = Colors.Red;
            }
            else if (bg == SquareBackground.Black)
            {
                brush.Color = Colors.LightGray;
            }
            else if (bg == SquareBackground.Move)
            {
                brush.Color = Colors.LightGreen;
            }
            else
            {
                brush.Color = Colors.White;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
