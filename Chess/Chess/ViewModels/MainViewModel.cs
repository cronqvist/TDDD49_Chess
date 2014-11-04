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
            PressedButton = new SimpleCommand(ExecutePressedButton, CanExecutePressedButton);

            Board = new Square[8][];
            for (int i = 0; i < 8; i++)
            {
                Board[i] = new Square[8];
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((j - i + 1) % 2 == 0)
                        Board[i][j] = new Square(new SolidColorBrush(Colors.White));
                    else
                        Board[i][j] = new Square(new SolidColorBrush(Colors.DarkGray));
                }
            }

            resetPieceses();
        }

        private void resetPieceses()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i][j].SetBackground(Backgrounds.Original);
                    Board[i][j].Piece = null;
                }
            }

            Board[0][0].Piece = new Rook(Player.White, new PiecePosition(0, 0));
            Board[1][0].Piece = new Knight(Player.White, new PiecePosition(1, 0));
            Board[2][0].Piece = new Bishop(Player.White, new PiecePosition(2, 0));
            Board[3][0].Piece = new Queen(Player.White, new PiecePosition(3, 0));
            Board[4][0].Piece = new King(Player.White, new PiecePosition(4, 0));
            Board[5][0].Piece = new Bishop(Player.White, new PiecePosition(5, 0));
            Board[6][0].Piece = new Knight(Player.White, new PiecePosition(6, 0));
            Board[7][0].Piece = new Rook(Player.White, new PiecePosition(7, 0));
            for (int i = 0; i < 8; i++)
            {
                Board[i][1].Piece = new Pawn(Player.White, new PiecePosition(i, 1));
            }

            Board[0][7].Piece = new Rook(Player.Black, new PiecePosition(0, 7));
            Board[1][7].Piece = new Knight(Player.Black, new PiecePosition(1, 7));
            Board[2][7].Piece = new Bishop(Player.Black, new PiecePosition(2, 7));
            Board[3][7].Piece = new Queen(Player.Black, new PiecePosition(3, 7));
            Board[4][7].Piece = new King(Player.Black, new PiecePosition(4, 7));
            Board[5][7].Piece = new Bishop(Player.Black, new PiecePosition(5, 7));
            Board[6][7].Piece = new Knight(Player.Black, new PiecePosition(6, 7));
            Board[7][7].Piece = new Rook(Player.Black, new PiecePosition(7, 7));
            for (int i = 0; i < 8; i++)
            {
                Board[i][6].Piece = new Pawn(Player.Black, new PiecePosition(i, 6));
            }
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
