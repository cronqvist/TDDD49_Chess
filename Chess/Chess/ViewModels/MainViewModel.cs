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
        public ICommand SquarePressed { get; private set; }

        public Square[][] Board { get; private set; }

        private List<Move> moves;
        private Square selectedSquare;

        public MainViewModel()
        {
            SquarePressed = new SimpleCommand(ExecuteSquarePressed, CanExecuteSquarePressed);

            initBoard();
        }

        #region helper_functions

        private void initBoard()
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

        #endregion

        #region commands

        private bool CanExecuteSquarePressed(object parameter)
        {
            return true;
        }

        private void ExecuteSquarePressed(object parameter)
        {
            Square square = parameter as Square;
            var piece = square.Piece;

            if (moves != null)
            {
                if (square.Background != square.OriginalBackground)
                {
                    foreach (var move in moves)
                    {
                        if (Board[move.Position.X][move.Position.Y] == square)
                        {
                            selectedSquare.Piece.Position = new PiecePosition(move.Position.X, move.Position.Y);
                            Board[move.Position.X][move.Position.Y].Piece = selectedSquare.Piece;
                            selectedSquare.Piece = null;

                            break;
                        }
                    }
                }

                foreach (var move in moves)
                {
                    Board[move.Position.X][move.Position.Y].SetBackground(Backgrounds.Original);
                }

                selectedSquare = null;
                moves = null;
            }
            else if (square.Piece != null)
            {
                selectedSquare = square;

                moves = piece.GetAvailableMoves();
                foreach (var move in moves)
                {
                    if (move.Type == MoveType.Normal)
                        Board[move.Position.X][move.Position.Y].SetBackground(Backgrounds.Move);
                    else if (move.Type == MoveType.Attack)
                        Board[move.Position.X][move.Position.Y].SetBackground(Backgrounds.Attacked);
                }
            }
        }
    }

    #endregion
}
