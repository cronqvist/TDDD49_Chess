using Chess.Model;
using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess.ViewModels
{
    public class MainViewModel
    {
        public ICommand PressedButton { get; private set; }

        public Square[][] Board { get; private set; }

        public MainViewModel()
        {
            Board = new Square[8][];
            for (int i = 0; i < 8; i++)
            {
                Board[i] = new Square[8];
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i][j] = new Square(new SolidColorBrush(Colors.Purple)) { Piece = new Pawn(Player.White, new PiecePosition(i, j)) };
                }
            }

            PressedButton = new SimpleCommand(ExecutePressedButton, CanExecutePressedButton);
        }

        private bool CanExecutePressedButton(object parameter)
        {
            return true;
        }

        private void ExecutePressedButton(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
