using System;
using System.Globalization;
using System.Windows.Data;
using Chess.Model;

namespace Chess.Converters
{
    public class SquareValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var squares = (Square[,]) value;

            //squares.Reverse();

            var temp = new Square[8][];
            for (int i = 0; i < 8; i++)
            {
                temp[i] = new Square[8];
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    temp[j][i] = squares[j, 7 - i];
                }
            }

            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}