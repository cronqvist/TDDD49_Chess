using Chess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Chess.Game
{
    public class GameEngine
    {
        private RuleEngine ruleEngine;

        public Square[][] Board { get; private set; }

        public Player Turn { get; private set; }

        private List<Move> moves;

        private Square selectedSquare;

        public GameEngine()
        {
            Turn = Player.White;

            ruleEngine = new RuleEngine(board);

            initBoard();
        }

        public void HandleInput(Square square)
        {
            var piece = square.Piece;

            if (piece != null && piece.Color == Turn) // a piece of correct turn was pressed
            {
                selectedSquare = square;

                if (moves != null) // if there already are moves, reset them
                {
                    resetBackgrounds(moves);
                }

                moves = piece.GetAvailableMoves(); // gets the new moves for the current piece
                foreach (var move in moves)
                {
                    if (move.Type == MoveType.Move)
                    {
                        Board[move.Position.X][move.Position.Y].Background = SquareBackground.Move;
                    }
                    else if (move.Type == MoveType.Attack)
                    {
                        Board[move.Position.X][move.Position.Y].Background = SquareBackground.Attacked;
                    }
                }
            }
            else if (square.Background != square.OriginalBackground) // if one of the moves was pressed
            {
                // valid square was pressed for move
                foreach (var move in moves)
                {
                    if (Board[move.Position.X][move.Position.Y] == square) // find the move
                    {
                        selectedSquare.Piece.Position = new PiecePosition(move.Position.X, move.Position.Y);
                        Board[move.Position.X][move.Position.Y].Piece = selectedSquare.Piece;
                        selectedSquare.Piece = null;

                        break;
                    }
                }

                swapTurn();
                resetBackgrounds(moves);
                selectedSquare = null;
                moves = null;
            }
        }

        private void resetPieceses()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i][j].ResetBackground();
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
                        Board[i][j] = new Square(SquareBackground.White);
                    else
                        Board[i][j] = new Square(SquareBackground.Black);
                }
            }

            resetPieceses();
        }
        private void swapTurn()
        {
            if (Turn == Player.White)
            {
                Turn = Player.Black;
            }
            else
            {
                Turn = Player.White;
            }
        }
        private void resetBackgrounds(List<Move> moves)
        {
            foreach (var move in moves)
            {
                Board[move.Position.X][move.Position.Y].ResetBackground();
            }
        }
    }
}
