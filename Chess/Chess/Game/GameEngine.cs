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
      
        //public Square[][] Board { get; private set; }

        
        private GameBoard _board;
        private RuleEngine ruleEngine;
        private List<Move> moves;
        private Square selectedSquare;

        public GameBoard Board { get {return _board;} }
        public Player Turn { get; private set; }


        public GameEngine()
        {
            Turn = Player.White;
            _board = new GameBoard();
            ruleEngine = new RuleEngine(Board);
        }

        public void HandleInput(Square square)
        {
            var piece = square.Piece;

            if (piece != null && piece.Color == Turn) // a piece of correct turn was pressed
            {
                selectedSquare = square;

                if (moves != null) // if there already are moves, reset them
                {
                    ResetBackgrounds(moves);
                }

                moves = piece.GetAvailableMoves(_board); // gets the new moves for the current piece
                foreach (var move in moves)
                {
                    SquareBackground bg = move.Type == MoveType.Move ? SquareBackground.Move : SquareBackground.Attacked;
                    Board.SetBackgroundAt(move.Position.X, move.Position.Y, bg );
                }
            }
            else if (square.Background != square.OriginalBackground) // if one of the moves was pressed
            {
                // valid square was pressed for move
                foreach (var move in moves)
                {
                    if (Board[move.Position.X, move.Position.Y] == square) // find the move
                    {
                        selectedSquare.Piece.Position = new PiecePosition(move.Position.X, move.Position.Y);
                        Board[move.Position.X,move.Position.Y].Piece = selectedSquare.Piece; //TODO: Play som fancy animation
                        selectedSquare.Piece = null;
                        break;
                    }
                }

                SwapTurn();
                ResetBackgrounds(moves);
                selectedSquare = null;
                moves = null;
            }
        }

 
        private void SwapTurn()
        {
            Turn = Turn == Player.White ? Player.Black : Player.White;
        }

        private void ResetBackgrounds(List<Move> moves)
        {
            foreach (var move in moves)
            {
                Board[move.Position.X,move.Position.Y].ResetBackground();
            }
        }
    }
}
